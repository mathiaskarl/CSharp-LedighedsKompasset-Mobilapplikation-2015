﻿

#pragma checksum "C:\Users\mathi_000\Documents\Visual Studio 2013\LedighedsKompasset\LedighedsApp\LedighedsApp\View\HandleNotificationView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "648A528A61DA206A74FDC9AC85FE77C2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LedighedsApp.View
{
    partial class HandleNotificationView : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 11 "..\..\..\View\HandleNotificationView.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 40 "..\..\..\View\HandleNotificationView.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.GoBack_OnTapped;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

