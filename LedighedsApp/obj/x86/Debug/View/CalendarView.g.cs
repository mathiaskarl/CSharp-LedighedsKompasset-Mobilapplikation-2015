﻿

#pragma checksum "C:\Users\mathi_000\Documents\Visual Studio 2013\LedighedsKompasset\LedighedsApp\LedighedsApp\View\CalendarView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7273590EBA45A280321DBC50838E29F5"
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
    partial class CalendarView : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 12 "..\..\..\View\CalendarView.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 79 "..\..\..\View\CalendarView.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.PreviousButton_Tapped;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 84 "..\..\..\View\CalendarView.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.NextButton_Tapped;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 92 "..\..\..\View\CalendarView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.CalendarTypeListView_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 43 "..\..\..\View\CalendarView.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.GoBack_OnTapped;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 52 "..\..\..\View\CalendarView.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.Grid_Tapped;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


