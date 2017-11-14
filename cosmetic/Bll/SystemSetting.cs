using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cosmetic.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Bll
{
    public static class SystemSettings
    {
        static SystemSettings()
        {
            Init();
            Load();
        }

        private static void Load()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var systems = db.SystemSettings.ToList();
                foreach (var item in systems)
                {
                    switch (item.Key)
                    {
                        case Enums.SystemSettingType.Banks:
                            _banks = JsonConvert.DeserializeObject<ObservableCollection<string>>(item.Value);
                            _banks.CollectionChanged += _banks_CollectionChanged;
                            break;
                        case Enums.SystemSettingType.Display:
                            {
                                _display = JsonConvert.DeserializeObject<ObservableCollection<int>>(item.Value);
                                _display.CollectionChanged += _display_CollectionChanged;
                            }
                            break;
                        case Enums.SystemSettingType.Mission:
                            {
                                _mission = JsonConvert.DeserializeObject<string>(item.Value);
                            }
                            break;
                        case Enums.SystemSettingType.Contact:
                            {
                                _contact = JsonConvert.DeserializeObject<string>(item.Value);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void _banks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Update(Enums.SystemSettingType.Banks, _banks);
        }

        private static ObservableCollection<string> _banks;

        public static ObservableCollection<string> Banks
        {
            get
            {
                return _banks;
            }
        }



        private static void _display_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Update(Enums.SystemSettingType.Display, _display);
        }

        private static ObservableCollection<int> _display;

        public static ObservableCollection<int> Display
        {
            get
            {
                return _display;
            }
        }


        //任务设置
        private static string _mission;

        public static string Mission
        {
            get
            {
                return _mission;
            }
            set
            {
                if (_mission != value)
                {
                    _mission = value;
                    Update(Enums.SystemSettingType.Mission, _mission);
                }
            }
        }

        //联系我们
        private static string _contact;

        public static string Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                if (_contact != value)
                {
                    _contact = value;
                    Update(Enums.SystemSettingType.Contact, _contact);
                }
            }
        }

        public static void Init()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var setting = db.SystemSettings.ToList();
                var init = new List<SystemSetting>();
                if (!setting.Any(s => s.Key == Enums.SystemSettingType.Banks))
                {
                    init.Add(new SystemSetting
                    {
                        Key = Enums.SystemSettingType.Banks,
                        Value = JsonConvert.SerializeObject(new string[0])
                    });
                }
                if (!setting.Any(s => s.Key == Enums.SystemSettingType.Display))
                {
                    init.Add(new SystemSetting
                    {
                        Key = Enums.SystemSettingType.Display,
                        Value = JsonConvert.SerializeObject(new int[0])

                    });
                }
                if (!setting.Any(s => s.Key == Enums.SystemSettingType.Mission))
                {
                    init.Add(new SystemSetting
                    {
                        Key = Enums.SystemSettingType.Mission,
                        Value = ""

                    });
                }
                if (!setting.Any(s => s.Key == Enums.SystemSettingType.Contact))
                {
                    init.Add(new SystemSetting
                    {
                        Key = Enums.SystemSettingType.Contact,
                        Value = ""

                    });
                }
                if (init.Count > 0)
                {
                    db.SystemSettings.AddRange(init);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 更新后Setting清空内存
        /// </summary>
        /// <param name="t"></param>
        /// <param name="o"></param>
        private static void Update(Enums.SystemSettingType t, object o)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var setting = db.SystemSettings.FirstOrDefault(s => s.Key == t);
                if (setting != null)
                {
                    setting.Value = JsonConvert.SerializeObject(o);
                }
                else
                {
                    db.SystemSettings.Add(new SystemSetting
                    {
                        Key = t,
                        Value = JsonConvert.SerializeObject(o)
                    });
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 清空内存
        /// </summary>
        public static void Clean()
        {
            Load();
        }
    }
}