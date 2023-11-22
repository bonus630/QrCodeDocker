﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace br.corp.bonus630.PluginLoader
{
    public class Loader
    {
        private string pluginFolder; 
        public Loader(string addonsPath)
        {
       
            pluginFolder = Path.Combine(addonsPath, "QrCodeDocker\\extras");
            if (!Directory.Exists(pluginFolder))
                Directory.CreateDirectory(pluginFolder);

        }
        public Assembly GetAssembly(PluginMap pluginMap)
        {
            try
            {
                return Assembly.LoadFrom(pluginMap.DllFile);
            }
            catch (Exception e)
            {
                throw e;
            }
            

        }
        public Type GetMainUIType(PluginMap pluginMap)
        {
            try
            {
                Assembly asm = GetAssembly(pluginMap);
                Type[] types = asm.GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (typeof(IPluginMainUI).IsAssignableFrom(types[j]) && !types[j].IsInterface)
                    {
                        return types[j];
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }
        public PluginMap GetPluginMap(string file)
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(file);
                Type[] types = asm.GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (typeof(IPluginCore).IsAssignableFrom(types[j]) && !types[j].IsInterface)
                    {
                        return new PluginMap(j,file, types[j].GetField("PluginDisplayName").GetValue(new object()).ToString());
                    }

                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
        public List<PluginMap> PluginList()
        {
            if (!Directory.Exists(pluginFolder))
                throw new Exception("Extras folder not found!");
            List<PluginMap> result = new List<PluginMap>();

            string[] files = Directory.GetFiles(pluginFolder, "*.dll");
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    PluginMap p = GetPluginMap(files[i]);
                    if(p!= null)
                        result.Add(p);
                    
                }
                catch(Exception e)
                {
                    throw e;
                }

            }
            result.Sort((x, y) => String.Compare(x.DisplayName, y.DisplayName));
            return result;
        }
        //public object GetUIControl(PluginMap plugin)
        //{
        //    return getInstance<IPluginUI>(plugin);
        //}
        public IPluginCore GetCore(PluginMap plugin)
        {
            try
            {
                return getInstance<IPluginCore>(plugin) as IPluginCore;
            }
            catch(Exception e)
            {
                throw e;
            }
            return null;
        }
        private object getInstance<T>(PluginMap plugin)
        {
            if (plugin == null)
                return null;

            Assembly asm = Assembly.LoadFrom(plugin.DllFile);
            Type[] types = asm.GetTypes();
            for (int j = 0; j < types.Length; j++)
            {
                if (typeof(T).IsAssignableFrom(types[j]) && !types[j].IsInterface)
                {
                    try
                    {
                        return Activator.CreateInstance(types[j]);
                    }
                    catch(TargetInvocationException e)
                    {
                        throw e;
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            return null;
        }
    }
}
