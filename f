[33mcommit 0893a3b4cda7c6bc14c47afd754d112c6a38c7c4[m[33m ([m[1;36mHEAD[m[33m, [m[1;31morigin/newPluginSystemDesigner[m[33m, [m[1;32mnewPluginSystemDesigner[m[33m)[m
Author: Reginaldo Santos <bonus630@gmail.com>
Date:   Mon Apr 18 17:04:52 2022 -0300

    continue changes in plugin system

[1mdiff --git a/.vs/VSWorkspaceState.json b/.vs/VSWorkspaceState.json[m
[1mnew file mode 100644[m
[1mindex 0000000..c8e662f[m
[1m--- /dev/null[m
[1m+++ b/.vs/VSWorkspaceState.json[m
[36m@@ -0,0 +1,5 @@[m
[32m+[m[32m{[m
[32m+[m[32m  "ExpandedNodes": [],[m
[32m+[m[32m  "SelectedNode": "\\QrCodeDocker.sln",[m
[32m+[m[32m  "PreviewInSolutionExplorer": false[m
[32m+[m[32m}[m
\ No newline at end of file[m
[1mdiff --git a/.vs/slnx.sqlite b/.vs/slnx.sqlite[m
[1mindex 1cfc1ae..8da9824 100644[m
Binary files a/.vs/slnx.sqlite and b/.vs/slnx.sqlite differ
[1mdiff --git a/PluginLoader/IPluginUI.cs b/PluginLoader/IPluginUI.cs[m
[1mindex 30d42f9..6a829f4 100644[m
[1m--- a/PluginLoader/IPluginUI.cs[m
[1m+++ b/PluginLoader/IPluginUI.cs[m
[36m@@ -4,22 +4,14 @@[m [mnamespace br.corp.bonus630.PluginLoader[m
 {[m
     public interface IPluginUI[m
     {[m
[31m-         //string PluginDisplayName{get;}[m
[31m-        [m
[31m-        void OnProgressChange(int progress);[m
[31m-        void OnFinishJob(object obj);[m
[31m-[m
         void ChangeLang(LangTagsEnum langTag);[m
         void SaveConfig();[m
         void LoadConfig();[m
         void DeleteConfig();[m
 [m
[31m-        event Action<object> FinishJob;[m
[31m-        event Action<string> AnyTextChanged;[m
[31m-        event Action<int> ProgressChange;[m
[31m-        event Action UpdatePreview;[m
         int Index { get; set; }[m
         string PluginDisplayName { get; }[m
[32m+[m[32m        object DataContext { get; set; }[m
 [m
     }[m
 }[m
[1mdiff --git a/PluginLoader/PluginCoreBase.cs b/PluginLoader/PluginCoreBase.cs[m
[1mindex b9e42c6..a568858 100644[m
[1m--- a/PluginLoader/PluginCoreBase.cs[m
[1m+++ b/PluginLoader/PluginCoreBase.cs[m
[36m@@ -5,12 +5,15 @@[m [musing System.Text;[m
 [m
 namespace br.corp.bonus630.PluginLoader[m
 {[m
[31m-    public abstract class PluginCoreBase : IPluginCore[m
[32m+[m[32m    public abstract class PluginCoreBase<T> : IPluginCore where T: class, new()[m
     {[m
         public event Action<object> FinishJob;[m
         public event Action<int> ProgressChange;[m
         public event Action UpdatePreview;[m
 [m
[32m+[m[32m        public abstract LangController Lang { get; set; }[m
[32m+[m[32m        public abstract string PluginDisplayName { get; }[m
[32m+[m
         protected virtual void OnFinishJob(object obj)[m
         {[m
             if (FinishJob != null)[m
[36m@@ -28,5 +31,19 @@[m [mnamespace br.corp.bonus630.PluginLoader[m
             if (UpdatePreview != null)[m
                 UpdatePreview();[m
         }[m
[32m+[m
[32m+[m[32m        public void ChangeLang(LangTagsEnum langTag, System.Reflection.Assembly assembly)[m
[32m+[m[32m        {[m
[32m+[m[32m            LangController Lang = LangController.CreateInstance(assembly, langTag);[m
[32m+[m[32m            Lang.AutoUpdateProperties();[m
[32m+[m[32m        }[m
[32m+[m[41m        [m
[32m+[m[32m        public IPluginUI CreateUIIntance()[m
[32m+[m[32m        {[m
[32m+[m[32m            IPluginUI ui = Activator.CreateInstance(typeof(T)) as IPluginUI;[m
[32m+[m[32m            ui.DataContext = this;[m
[32m+[m[32m            return ui;[m
[32m+[m[32m        }[m
[32m+[m
     }[m
 }[m
