using AutoMapper;
using Fakka.Core.Actions;
using Fakka.Core.Business.Enums;
using Fakka.Core.Business.Interfaces;
using Fakka.Core.Business.Models;
using Fakka.Core.Enums;
using Fakka.Core.Exceptions;
using Fakka.Core.Interfaces;
using Fakka.Core.Managers;
using Fakka.Core.Models;
using Fakka.Core.PageViewModels;
using Fakka.Core.Utilities;
using Fakka.Core.Validations;
using Fakka.Pos.EventArguments;
using Fakka.Pos.Models;
using Fakka.Pos.Resources;
using Fakka.Pos.Routes;
using Fakka.Pos.Validations.Commands;
using Microsoft.AppCenter.Crashes;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace Fakka.Pos.ViewModels
{
    public class MainPageViewModel : BasePageViewModel
    {
        #region Private fields
        private IBluetoothHandler bluetoothHandler;
        private List<ChildProfile> _childrenProfiles = new List<ChildProfile>();
        private School school;
        private UserSession userSession;
        private List<OnlineOrder> onlineOrders = new List<OnlineOrder>();
        private List<StockItem> bannedProducts = new List<StockItem>();
        private bool bluetoothPermissionGranted = false;
        private TodayOnlineOrder previouslySelectedOrder;
        #endregion

        #region UI Properties
        private ObservableCollection<StockItem> _stockItems;
        public ObservableCollection<StockItem> StockItems
        {
            get => _stockItems;
            set
            {
                _stockItems = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<LineItem> _transactionItems = new ObservableCollection<LineItem>();
        public ObservableCollection<LineItem> TransactionItems
        {
            get => _transactionItems;
            set
            {
                SetProperty(ref _transactionItems, value);
                RaisePropertyChanged(nameof(CashTransactionAllowed));
                RaisePropertyChanged(nameof(BalanceTransactionAllowed));
            }
        }
        public bool HasTransactionItems => TransactionItems?.Any() ?? false;

        public bool CashTransactionAllowed => HasTransactionItems && (SelectedChild == null || SelectedChild.CurrentBalance <= 0);
        public bool BalanceTransactionAllowed => HasTransactionItems && (SelectedChild?.CurrentBalance > 0);
        ObservableCollection<Grade> _grades;
        public ObservableCollection<Grade> Grades
        {
            get => _grades;
            set
            {
                _grades = value;
                RaisePropertyChanged();
            }
        }
        private bool _searching;
        public bool Searching
        {
            get => _searching;
            set => SetProperty(ref _searching, value);
        }

        private ObservableCollection<ChildProfile> _searchChildren;
        public ObservableCollection<ChildProfile> SearchChildren
        {
            get => _searchChildren;
            set
            {
                _searchChildren = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ChildProfile> _bluetoothChildren = new ObservableCollection<ChildProfile>();
        public ObservableCollection<ChildProfile> BluetoothChildren
        {
            get => _bluetoothChildren;
            set => SetProperty(ref _bluetoothChildren, value);
        }

        private Grade _selectedGrade;
        public Grade SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                SetProperty(ref _selectedGrade, value);
                Grade.Value = value;

            }
        }

        private bool _bluetoothActive;
        public bool BluetoothActive
        {
            get => _bluetoothActive;
            set => SetProperty(ref _bluetoothActive, value);
        }
        private ChildProfile _selectedChild;
        public ChildProfile SelectedChild
        {
            get => _selectedChild;
            set
            {
                SetProperty(ref _selectedChild, value);
                RaisePropertyChanged(nameof(CashTransactionAllowed));
                RaisePropertyChanged(nameof(BalanceTransactionAllowed));
            }

        }
        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        private string _schoolLogo;
        public string SchoolLogo
        {
            get => _schoolLogo;
            set => SetProperty(ref _schoolLogo, value);
        }

        private string _schoolName;
        public string SchoolName
        {
            get => _schoolName;
            set => SetProperty(ref _schoolName, value);
        }

        private string _educationalDepartmentName;
        public string EducationalDepartmentName
        {
            get => _educationalDepartmentName;
            set => SetProperty(ref _educationalDepartmentName, value);
        }

        private ObservableCollection<BaseUiModel> _todayOnlineOrders = new ObservableCollection<BaseUiModel>();
        public ObservableCollection<BaseUiModel> TodayOnlineOrders
        {
            get => _todayOnlineOrders;
            set => SetProperty(ref _todayOnlineOrders, value, () => RaisePropertyChanged(nameof(TotalOnlineOrders)));
        }
        public int TotalOnlineOrders => TodayOnlineOrders.Count;
        public bool HasOnlineOrder => this.onlineOrders?.Any(order => order.ChildId.ToString().ToLower() == SelectedChild?.Id) ?? false;

        #endregion

        #region Validatable Objects
        private ValidatableObject<string> _searchName = new ValidatableObject<string>();
        public ValidatableObject<string> SearchName
        {
            get => _searchName;
            set => SetProperty(ref _searchName, value);
        }

        private ValidatableObject<string> _searchCode = new ValidatableObject<string>();
        public ValidatableObject<string> SearchCode
        {
            get => _searchCode;
            set => SetProperty(ref _searchCode, value);
        }

        private ValidatableObject<Grade> _grade = new ValidatableObject<Grade>();
        public ValidatableObject<Grade> Grade
        {
            get => _grade;
            set => SetProperty(ref _grade, value);
        }
        #endregion

        #region Commands

        public DelegateCommand CodeValidaionCommand { get; set; } //new RequiredValidationCommand<string>(SearchCode, UiResources.SearchWithCode);
        public DelegateCommand GradeValidationCommand { get; set; }// new RequiredValidationCommand<Grade>(Grade, UiResources.Grade);
        public DelegateCommand<object> ChildSelectedCommand => new BaseCommandHandler<object>(this, (child) =>
        {
            if (child == null)
                SelectedChild = null;

            SelectedChild = (ChildProfile)child;

            //reset banned products
            foreach (var product in this.bannedProducts)
                product.IsBanned = false;

            bannedProducts?.Clear();

            if (SelectedChild.BannedProducts != null)
                foreach (var product in SelectedChild.BannedProducts)
                {
                    var bannedProduct = StockItems.FirstOrDefault(prod => prod.Id.ToLower() == product.Id.ToLower());
                    if (bannedProduct == null)
                        continue;

                    bannedProduct.IsBanned = true;
                    this.bannedProducts.Add(bannedProduct);
                }

            if (TransactionItems != null) //clear previously selected items

                foreach (var item in TransactionItems)
                {
                    var stockItem = StockItems.First(i => i.Id.ToLower() == item.Id.ToLower());
                    stockItem.IsSelected = false;
                }


            TransactionItems.Clear();



            //If the child has an online order set the transaction items to items in the order
            if (HasOnlineOrder)
            {
                var order = this.onlineOrders.First(o => o.ChildId.ToString().ToLower() == SelectedChild.Id);
                var lineItems = Mapper.Map<IList<TransactionItem>, IList<OnlineOrderLineItem>>(order.OrderItems);
                TransactionItems?.Clear();
                TransactionItems.AddRange(lineItems);
            }

        });

        public DelegateCommand<object> StockItemSelected => new BaseCommandHandler<object>(this, (obj) =>
        {

            var item = (StockItem)obj;

            if (item.IsSelected)
            {
                bool isBannedProduct = SelectedChild?.BannedProducts != null && SelectedChild.BannedProducts.Any(prod => prod.Id.ToLower() == item.Id.ToLower());

                if (isBannedProduct)
                {
                    item.IsSelected = false;
                    throw new BusinessException(1, UiResources.ProductBannedMessage, UiResources.ProductBannedMessage);
                }

                if (HasOnlineOrder)
                {
                    item.IsSelected = false;
                    throw new BusinessException(1, UiResources.ChildHasOnlineOrderErrorMessage, UiResources.ChildHasOnlineOrderErrorMessage);
                }
                var transactionItem = Mapper.Map<LineItem>(item);
                TransactionItems.Insert(0, transactionItem);
            }
            else
                TransactionItems.Remove(TransactionItems.First(i => i.Id.ToLower() == item.Id.ToLower()));

        });

        private object _selectedTodayOrder;
        public object SelectedTodayOrder
        {
            get => _selectedTodayOrder;
            set => SetProperty(ref _selectedTodayOrder, value, SelectedTodayOrderChanged);
        }

        public DelegateCommand<ValidatableObject<string>> NameChangedCommand => new BaseCommandHandler<ValidatableObject<string>>(this,
            (name) =>
            {
                _searchName.Value = _searchName.Value?.Trim() ?? string.Empty;
                SearchChildren = new ObservableCollection<ChildProfile>(_childrenProfiles.Where(profile =>
               (profile.Name.ToLower().Contains(_searchName.Value.ToLower())
                || profile.ParentName.ToLower().Contains(_searchName.Value.ToLower())) && profile.GradeId.ToLower() == _selectedGrade.Id.ToLower())
                    .ToList());
            },
            () =>
            {
                bool validGrade = Grade?.Value != null;
                bool validName = !SearchName?.Value.IsNullOrWhiteSpace() ?? false;

                Searching = validGrade && validName;
                if (!validName)
                {
                    View.Alert(UiResources.Error, UiResources.NameRequiredErrorMessage);
                }
                else if (!validGrade)
                {
                    View.Alert(UiResources.Error, UiResources.SelectGradeMessage);
                }

                return validGrade && validName;
            });

        public DelegateCommand<ValidatableObject<string>> CodeChangedCommand => new BaseCommandHandler<ValidatableObject<string>>(this,
            (code) =>
            {
                Searching = true;
                _searchCode.Value = _searchCode.Value?.Trim() ?? string.Empty;
                SearchChildren = new ObservableCollection<ChildProfile>(_childrenProfiles.Where(profile =>
                profile.Code.ToString() == _searchCode.Value.ToLower())
                .ToList());

            });
        public DelegateCommand<object> LineItemsIncreasedCommand => new BaseCommandHandler<object>(this, (obj) =>
        {
            var item = (LineItem)obj;

            if (item.ItemType == ItemType.Product && item.Quantity == item.MaxQuantity)
                return;

            item.Quantity++;
        });

        public DelegateCommand<object> LineItemsDecreasedCommand => new BaseCommandHandler<object>(this, (obj) =>
        {
            var item = (LineItem)obj;
            item.Quantity--;
            if (item.Quantity <= 0)
            {
                TransactionItems.Remove(TransactionItems.First(i => i.Id.ToLower() == item.Id.ToLower()));
                var stockItem = StockItems.First(i => i.Id.ToLower() == item.Id.ToLower());
                stockItem.IsSelected = false;
            }
        });

        public DelegateCommand BluetoothActivationCommand => new BaseCommandHandler(this, async () =>
        {
            if (BluetoothActive)
                this.NativeService.DisableBluetooth();
            else
                this.NativeService.EnableBluetooth();

            BluetoothActive = !BluetoothActive;

            if (!BluetoothActive)
                BluetoothChildren.Clear();
        });

        public DelegateCommand TransactionCommand => new BaseCommandHandler(this, async () =>
        {
            ShowLoading();
            List<TransactionItem> items = new List<TransactionItem>();
            foreach (var item in TransactionItems)
            {
                var transItem = Mapper.Map<TransactionItem>(item);
                items.Add(transItem);
            }

            await DataContext.Get<ISalesTransactionContext>().CreateFullTransaction(items, SelectedChild.Id);
            //await View.Alert(string.Empty, UiResources.TransactionCompleted);
            ClearAll();
            HideLoading();
            await UpdateStockQuantity();
            await LoadChildrenData();
        });
        public DelegateCommand CashTransactionCommand => new BaseCommandHandler(this, async () =>
        {
            ShowLoading();
            List<TransactionItem> items = new List<TransactionItem>();
            foreach (var item in TransactionItems)
            {
                var transItem = Mapper.Map<TransactionItem>(item);
                items.Add(transItem);
            }
            await DataContext.Get<ISalesTransactionContext>().CreateFullCashTransaction(items, SelectedChild?.Id);
            //await View.Alert(string.Empty, UiResources.TransactionCompleted);
            ClearAll();
            HideLoading();
            await LoadStockItemsData();
        });
        public DelegateCommand CancelCommand => new BaseCommandHandler(this, () =>
        {
            ClearAll();
        });

        public DelegateCommand LogoutCommand => new BaseCommandHandler(this, async () =>
        {
            ShowLoading();
            Container.Resolve<IStorageManager>().DeleteAll();
            SessionManager.EndSession();
            StateManager.Instance.DeleteAll();
            await NavigationService.NavigateAsync($"/{AppRoutes.MainNavigation}/{AppRoutes.Login}");
            HideLoading();
        });
        #endregion
        public MainPageViewModel(IContainerProvider container, IBluetoothHandler bluetoothHandler, INavigationService navigationService, IMapper mapper)
            : base(container, navigationService, mapper)
        {
            this.bluetoothHandler = bluetoothHandler;
            CodeValidaionCommand = new RequiredValidationCommand<string>(SearchCode, UiResources.SearchWithCode, string.Empty);
            GradeValidationCommand = new RequiredValidationCommand<Grade>(Grade, UiResources.Grade, string.Empty);
        }

        public async override Task OnNavigation(INavigationParameters parameters, NavigationMode navigationMode)
        {
            this.bluetoothHandler.Ble.StateChanged += SearchForBluetotthDevices;
            this.bluetoothHandler.BleAdapter.DeviceDiscovered += AddChildrenFromBluetooth;
            TransactionItems.CollectionChanged += TransactionItemsChanged;
            TodayOnlineOrders.CollectionChanged += TodaysOrdersChanged;
            ShowLoading(UiResources.LoadingProducts);
            this.userSession = await this.SessionManager.GetCurrentSession();

            var stockItemsTask = LoadStockItemsData();
            var childrenTask = LoadChildrenData();
            var gradesTask = LoadGradesData();
            var onlineOrdersTask = LoadOnlineOrdersData();
            var schoolTask = LoadSchoolData();
            Task[] loadingTasks = new Task[] { stockItemsTask, childrenTask, gradesTask, onlineOrdersTask, schoolTask };

            await Task.WhenAll(loadingTasks);
            HideLoading();
            ActivateBluetoothSearch();

            Device.StartTimer(TimeSpan.FromSeconds(60), () =>
            {
                Task.Run(async () =>
                {
                    if (!BluetoothActive || BluetoothChildren == null || !BluetoothChildren.Any() || !initialScanDone)
                        return;

                    if (this.bluetoothPermissionGranted)
                    {
                        this.bluetoothHandler.BleAdapter.DiscoveredDevices.Clear();
                        await this.bluetoothHandler.BleAdapter.StartScanningForDevicesAsync();
                    }
                    else
                        return;


                    var discoveredMacsList = this.bluetoothHandler.BleAdapter.DiscoveredDevices.Select(device =>
                    {
                        if (ApplicationManager.Instance.GetTerminalInfo().TerminalCode == TerminalCode.Android)
                        {
                            return Convert.ToInt64(device.NativeDevice.ToString().Replace(":", ""), 16)
                                 .ToString().ToLower();
                        }
                        else
                        {
                            return device.Id.ToString().Replace("-", "").ToLower();
                        }
                    }).ToList();

                    var outOfRangeChildren = BluetoothChildren.Where(child => !discoveredMacsList.Contains(child.DeviceMacAddress.ToLower()));

                    if (!outOfRangeChildren.Any())
                        return;

                    Device.BeginInvokeOnMainThread(() => BluetoothChildren.RemoveRange(outOfRangeChildren));

                });

                return true;

            });
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            this.bluetoothHandler.Ble.StateChanged -= SearchForBluetotthDevices;
            this.bluetoothHandler.BleAdapter.DeviceDiscovered -= AddChildrenFromBluetooth;
            TransactionItems.CollectionChanged -= TransactionItemsChanged;
            TodayOnlineOrders.CollectionChanged -= TodaysOrdersChanged;
        }


        private bool initialScanDone;
        private async Task ActivateBluetoothSearch()
        {
            BluetoothActive = this.bluetoothHandler.Ble.IsOn;
            if (!BluetoothActive)
            {
                initialScanDone = true;
                return;
            }

            this.bluetoothPermissionGranted = await PermissionHandler.CheckPermissions(Permission.LocationWhenInUse);

            if (BluetoothActive && this.bluetoothPermissionGranted)
                await this.bluetoothHandler.BleAdapter.StartScanningForDevicesAsync();

            initialScanDone = true;
        }


        private void TransactionItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(CashTransactionAllowed));
            RaisePropertyChanged(nameof(BalanceTransactionAllowed));
        }

        private async Task LoadChildrenData()
        {
            var childrenResponse = await DataContext.Get<IOdataContext>().GetChildrenDetails();
            _childrenProfiles = childrenResponse.Data.Value;
        }
        private async Task LoadGradesData()
        {
            var gradeResponse = await DataContext.Get<IGradeContext>().GetGradesBySchool(this.userSession.SchoolId);
            Grades = new ObservableCollection<Grade>(gradeResponse.Data);
        }
        private async Task LoadOnlineOrdersData()
        {
            var userSession = await SessionManager.GetCurrentSession();
            var response = await DataContext.Get<IOdataContext>().GetValidOnlineOrderBySchool(userSession.SchoolId);

            this.onlineOrders = new List<OnlineOrder>(response.Data.Value);
            TodayOnlineOrders?.Clear();

            foreach (var order in this.onlineOrders)
            {
                var todayOrder = Mapper.Map<TodayOnlineOrder>(order);
                todayOrder = Mapper.Map(order, todayOrder);

                TodayOnlineOrders.Add(todayOrder);
            }
        }

        private async Task LoadStockItemsData()
        {
            var itemsResponse = await DataContext.Get<IOdataContext>().GetSchoolStockItems(this.userSession.SchoolId);

            StockItems = new ObservableCollection<StockItem>(itemsResponse.Data.Value);

            Task.Run(async () =>
            {
                
                    foreach (var item in itemsResponse.Data.Value)
                    try
                    {
                        item.LocalImage = await CacheMedia(item.Image, $"{item.GetType().Name}_{item.Id}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Caching {item.Name} image failed");
                        Crashes.TrackError(ex);
                    }

                }, CancellationToken.None);
        }
        private async Task UpdateStockQuantity()
        {
            var itemsResponse = await DataContext.Get<IOdataContext>().GetSchoolStockItems(this.userSession.SchoolId);

            foreach (var item in itemsResponse.Data.Value)
            {
                var stockItem = StockItems.FirstOrDefault(i => i.Id == item.Id);
                if (stockItem == null)
                {
                    StockItems.Add(item);
                    continue;
                }

                stockItem.Quantity = item.Quantity;
            }
        }

        private async Task LoadSchoolData()
        {
            var response = await DataContext.Get<IOdataContext>().GetSchoolById(this.userSession.SchoolId);
            if (response.SuccessCode != 0)
                return;

            this.school = response.Data.Value.FirstOrDefault();

            if (school == null)
                return;

            SchoolLogo = this.school.Image;
            SchoolName = this.school.Name;
            EducationalDepartmentName = this.school.EducationalDepartmentName;
        }


        private void SelectedTodayOrderChanged()
        {
            if (SelectedTodayOrder == null || SelectedTodayOrder is OrderTransaction)
                return;

            if (this.previouslySelectedOrder != null)
                this.previouslySelectedOrder.IsSelected = false;



            //Clear all expanded items
            List<BaseUiModel> orders = TodayOnlineOrders.ToList();
            TodayOnlineOrders.RemoveRange(trans => trans is OrderTransaction);
            var order = (TodayOnlineOrder)SelectedTodayOrder;

            if (previouslySelectedOrder == order)
            {
                this.previouslySelectedOrder = null;
                return;
            }

            this.previouslySelectedOrder = order;


            order.IsSelected = true;
            int itemsCount = order.OrderItems.Count;
            int orderIndex = TodayOnlineOrders.IndexOf(order);
            bool lastOrder = orderIndex == TodayOnlineOrders.Count - 1;

            //We add this displacment to make up for the fact that we can't have empty items in a list and we need the order items to always be a raw under

            if (orderIndex % 2 == 0 && itemsCount % 2 != 0 && !lastOrder)
            {
                int cellItemDisplacment = orderIndex + 2;
                TodayOnlineOrders.Insert(cellItemDisplacment, new OrderTransaction());
            }
            else if (orderIndex % 2 != 0 && itemsCount % 2 != 0 || orderIndex % 2 == 0 && lastOrder)
            {
                int postCellDisplacment = orderIndex + 1;
                TodayOnlineOrders.Insert(postCellDisplacment, new OrderTransaction());
            }






            for (int i = 0; i < itemsCount; i++)
            {
                var orderItem = Mapper.Map<OrderTransaction>(order.OrderItems[i]);
                var stockiItem = StockItems.FirstOrDefault(item => new Guid(item.Id) == orderItem.ItemId);
                orderItem.Image = stockiItem?.Image;

                if (orderIndex % 2 != 0)
                    TodayOnlineOrders.Insert(orderIndex + 1, orderItem);
                else
                    TodayOnlineOrders.Insert(orderIndex + 2, orderItem);

            }

        }

        private void TodaysOrdersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(TotalOnlineOrders));
        }

        private async void SearchForBluetotthDevices(object sender, BluetoothStateChangedArgs e)
        {
            if (e.NewState == BluetoothState.On)
            {
                BluetoothChildren?.Clear();
                this.bluetoothHandler.BleAdapter.DiscoveredDevices?.Clear();

                this.bluetoothPermissionGranted = await PermissionHandler.CheckPermissions(Permission.LocationWhenInUse);
                if (!this.bluetoothPermissionGranted)
                    return;
                this.bluetoothHandler.BleAdapter.StartScanningForDevicesAsync();
            }
        }

        private void AddChildrenFromBluetooth(object sender, DeviceEventArgs e)
        {
            try
            {
                if (e.Device?.NativeDevice != null)
                {
                    string macAddress = string.Empty;

                    if (ApplicationManager.Instance.GetTerminalInfo().TerminalCode == TerminalCode.Android)
                    {
                        macAddress = Convert.ToInt64(e.Device.NativeDevice.ToString().Replace(":", ""), 16)
                             .ToString();
                        Debug.WriteLine($"Discovered device with MAC address {macAddress}");
                    }
                    if (ApplicationManager.Instance.GetTerminalInfo().TerminalCode == TerminalCode.Ios)
                    {
                        macAddress = e.Device.Id.ToString().Replace("-", "");
                    }

                    var child = this._childrenProfiles.FirstOrDefault(c => c.DeviceMacAddress?.ToLower() == macAddress);

                    if (child == null)
                        return;

                    bool alreadyAdded = BluetoothChildren.Any(c => c.Id == child.Id);

                    if (alreadyAdded)
                        return;

                    BluetoothChildren.Add(child);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private void ClearAll()
        {
            if (bannedProducts != null)
                foreach (var product in bannedProducts)
                    product.IsBanned = false;

            bannedProducts?.Clear();

            SearchChildren = null;

            foreach (var item in TransactionItems)
            {
                var stockItem = StockItems.First(i => i.Id.ToLower() == item.Id.ToLower());
                stockItem.IsSelected = false;
            }
            TransactionItems?.Clear();
            SelectedChild = null;

            if (SearchCode != null)
                SearchCode.Value = string.Empty;

            if (SearchName != null)
                SearchName.Value = string.Empty;

            if (SelectedGrade != null)
                SelectedGrade = null;
        }

    }
}
