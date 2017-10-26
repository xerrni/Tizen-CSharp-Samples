/*
 *  Copyright (c) 2017 Samsung Electronics Co., Ltd All Rights Reserved
 *
 *  Contact: Ernest Borowski <e.borowski@partner.samsung.com>
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License
 *
 *
 * @file        Certificates.cs
 * @author      Ernest Borowski (e.borowski@partner.samsung.com)
 * @version     1.0
 * @brief       This file contains classes used in ListView grupping (UI)
 */

using System.Collections.Generic;

namespace SecureRepository.Tizen
{
    public enum AliasType { Data,Key,Certificate }
    public class PageModel //PageModel will be stored inside PageTypeGroup, and then displayed in listView
    {
        public string Alias { get; }
        public AliasType Type { get; }
        public PageModel(string alias,AliasType type)
        {
            Alias = alias;
            Type = type;
        }
    }
    public class PageTypeGroup : List<PageModel>
    {
        public string Title { get; }
        public string ShortName { get; }
        public PageTypeGroup(AliasType aliasType)
        {
            switch(aliasType)
            {
                case AliasType.Certificate:
                    Title = "Certificate";
                    ShortName = "C";
                    break;
                case AliasType.Key:
                    Title = "Key";
                    ShortName = "K";
                    break;
                case AliasType.Data:
                    Title = "Data";
                    ShortName = "D";
                    break;
            }
        }        
    }
}
