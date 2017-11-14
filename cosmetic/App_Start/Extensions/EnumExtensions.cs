﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cosmetic
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取Enum的display name，没有的送货返回枚举本身名字
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDisplayName(this System.Enum enumValue)
        {
            return ((DisplayAttribute)(enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()?
                            .GetCustomAttributes(typeof(DisplayAttribute), false)[0])
                            ).Name ?? enumValue.ToString();
        }
    }
}