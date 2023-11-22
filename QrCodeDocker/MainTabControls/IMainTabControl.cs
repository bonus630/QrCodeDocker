using System;

namespace br.corp.bonus630.QrCodeDocker.MainTabControls
{
    interface IMainTabControl
    {
        event Action<string> AnyTextChanged;
        string FormatedText { get; }
        object DataContext { set; }

        void LoadLang(string lang);
    }
}
