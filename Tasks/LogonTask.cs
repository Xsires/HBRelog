﻿/*
Copyright 2012 HighVoltz

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using HighVoltz.Controls;
using HighVoltz.Settings;

namespace HighVoltz.Tasks
{
    public class LogonTask : BMTask
    {
        public LogonTask()
        {
            HonorbuddyPath = CustomClass = ProfilePath = BotBase = Server = CharacterName = "";
        }
        [XmlIgnore]
        public override string Name
        {
            get { return "Logon"; }
        }

        public string CharacterName { get; set; }
        public string Server { get; set; }
        public string BotBase { get; set; }
        public string CustomClass { get; set; }
        [HighVoltz.Controls.TaskEditor.CustomTaskEditControl(typeof(ProfilePathEditControl))]
        public string ProfilePath { get; set; }
        [HighVoltz.Controls.TaskEditor.CustomTaskEditControl(typeof(HBPathEditControl))]
        public string HonorbuddyPath { get; set; }
        public override void Pulse()
        {
            var wowSettings = Profile.Settings.WowSettings.ShadowCopy();
            var hbSettings = Profile.Settings.HonorbuddySettings.ShadowCopy();

            if (!string.IsNullOrEmpty(CharacterName))
                wowSettings.CharacterName = CharacterName;
            if (!string.IsNullOrEmpty(Server))
                wowSettings.CharacterName = Server;

            if (!string.IsNullOrEmpty(BotBase))
                hbSettings.BotBase = BotBase;
            if (!string.IsNullOrEmpty(ProfilePath))
                hbSettings.HonorbuddyProfile = ProfilePath;
            if (!string.IsNullOrEmpty(CustomClass))
                hbSettings.CustomClass = CustomClass;
            if (!string.IsNullOrEmpty(HonorbuddyPath))
                hbSettings.HonorbuddyPath = HonorbuddyPath;
            Profile.Log("Logging on new character.");
            // exit wow and honorbuddy
            Profile.TaskManager.HonorbuddyManager.Stop();
            Profile.TaskManager.WowManager.Stop();
            // assign new settings
            Profile.TaskManager.HonorbuddyManager.SetSettings(hbSettings);
            Profile.TaskManager.WowManager.SetSettings(wowSettings);
            Profile.TaskManager.WowManager.Start();
            IsDone = true;
        }

        public class ProfilePathEditControl : FileInputBox, HighVoltz.Controls.TaskEditor.ICustomTaskEditControlDataBound
        {
            LogonTask _task;
            public ProfilePathEditControl()
            {
                Title = "Browse to and select your profile";
                DefaultExt = ".xml";
                Filter = ".xml|*.xml";
            }
            void TaskEditor.ICustomTaskEditControlDataBound.SetBinding(BMTask source, string path)
            {
                _task = (LogonTask)source;
                // binding issues.. so just hooking an event.
                // Binding binding = new Binding(path);
                // binding.Source = source;
                // SetBinding(FileNameProperty, binding);
            }

            void TaskEditor.ICustomTaskEditControlDataBound.SetValue(object value)
            {
                FileName = value.ToString();
                FileNameChanged += ProfilePathEditControlFileNameChanged;
            }

            void ProfilePathEditControlFileNameChanged(object sender, System.Windows.RoutedEventArgs e)
            {
                _task.ProfilePath = FileName;
            }
        }

        public class HBPathEditControl : FileInputBox, HighVoltz.Controls.TaskEditor.ICustomTaskEditControlDataBound
        {
            LogonTask _task;
            public HBPathEditControl()
            {
                Title = "Browse to and your Honorbuddy .exe";
                DefaultExt = ".exe";
                Filter = ".exe|*.exe";
            }
            void TaskEditor.ICustomTaskEditControlDataBound.SetBinding(BMTask source, string path)
            {
                _task = (LogonTask)source;
                // binding issues.. so just hooking an event.
                // Binding binding = new Binding(path);
                // binding.Source = source;
                // SetBinding(FileNameProperty, binding);
            }

            void TaskEditor.ICustomTaskEditControlDataBound.SetValue(object value)
            {
                FileName = value.ToString();
                FileNameChanged += ProfilePathEditControlFileNameChanged;
            }

            void ProfilePathEditControlFileNameChanged(object sender, System.Windows.RoutedEventArgs e)
            {
                _task.ProfilePath = FileName;
            }
        }
    }
}
