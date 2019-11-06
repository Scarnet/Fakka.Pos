using System;
using System.Threading.Tasks;
using Fakka.Core.Interfaces;
using Fakka.Core.PageViewModels;
using Fakka.Core.Utilities;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;

namespace Fakka.Core.Actions
{
    public class BaseCommandHandler : DelegateCommand
    {

        public BaseCommandHandler(BasePageViewModel viewModel, Action action, Func<bool> canExecute = null) : base(
            () =>
            {
                try
                {
                    if (canExecute == null || canExecute())
                        action();
                }
                catch (Exception ex)
                {
                    viewModel.HideLoading();
                    ExceptionsHandler.Handle(ex, viewModel);
                }
            }, () => true)
        {
        }

        public BaseCommandHandler(BasePageViewModel viewModel, Func<Task> action, Func<bool> canExecute = null)
            : base(async () =>
            {
                try
                {
                    if (canExecute == null || canExecute())
                    {
                        await action();
                    }
                }
                catch (Exception ex)
                {
                    viewModel.HideLoading();
                    ExceptionsHandler.Handle(ex, viewModel);
                }
            })
        {
        }

    }

    public class BaseCommandHandler<T> : DelegateCommand<T>
    {
        public BaseCommandHandler(BasePageViewModel viewModel, Action<T> action, Func<bool> canExecute = null) : base(
            (obj) =>
            {
                try
                {
                    if (canExecute == null || canExecute())
                        action(obj);
                }
                catch (Exception ex)
                {
                    viewModel.HideLoading();
                    ExceptionsHandler.Handle(ex, viewModel);
                }
            }, (obj) =>
            {
                if (canExecute != null)
                  return  canExecute();

                return true; 
            })
        {

        }

        public BaseCommandHandler(BasePageViewModel viewModel, Func<T, Task> action, Func<bool> canExecute = null) : base(
            async (obj) =>
            {
                try
                {
                    if (canExecute == null || canExecute())
                        await action(obj);
                }
                catch (Exception ex)
                {
                    viewModel.HideLoading();
                    ExceptionsHandler.Handle(ex, viewModel);
                }
            }, (obj) =>
            {
                if (canExecute != null)
                    return canExecute();

                return true;
            })
        {

        }
    }
}