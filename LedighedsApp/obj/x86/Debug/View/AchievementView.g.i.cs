﻿

#pragma checksum "C:\Users\mathi_000\Documents\Visual Studio 2013\LedighedsKompasset\LedighedsApp\LedighedsApp\View\AchievementView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F5D1C646B5E52EA8B0BC6A47A76B057F"
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
    partial class AchievementView : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::LedighedsApp.Model.Assets.DateTimeConverter DateTimeConverter; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::LedighedsApp.Model.Assets.AchievementImagePath ImagePathConverter; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::LedighedsApp.Model.Assets.HighLightColor HighLightColor; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Grid MainGrid; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView EarnedAchievementsList; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView UnearnedAchievementsList; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///View/AchievementView.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            DateTimeConverter = (global::LedighedsApp.Model.Assets.DateTimeConverter)this.FindName("DateTimeConverter");
            ImagePathConverter = (global::LedighedsApp.Model.Assets.AchievementImagePath)this.FindName("ImagePathConverter");
            HighLightColor = (global::LedighedsApp.Model.Assets.HighLightColor)this.FindName("HighLightColor");
            MainGrid = (global::Windows.UI.Xaml.Controls.Grid)this.FindName("MainGrid");
            EarnedAchievementsList = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("EarnedAchievementsList");
            UnearnedAchievementsList = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("UnearnedAchievementsList");
        }
    }
}



