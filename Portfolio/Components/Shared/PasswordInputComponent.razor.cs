using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Linq.Expressions;

namespace Portfolio.Components.Shared
{
    public partial class PasswordInputComponent
    {
        [Parameter, EditorRequired]
        public required string Value { get; set; }
        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }
        [Parameter]
        public Expression<Func<string>>? For { get; set; }

        protected InputType PasswordInput = InputType.Password;
        protected string VisibilityIcon = Icons.Material.Filled.VisibilityOff;
        protected Color IconColor = Color.Error;

        private bool _isShow;

        protected void ToggleVisibility()
        {
            if (_isShow)
            {
                _isShow = false;
                VisibilityIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
                IconColor = Color.Error;
            }
            else
            {
                _isShow = true;
                VisibilityIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
                IconColor = Color.Success;
            }
        }
    }
}