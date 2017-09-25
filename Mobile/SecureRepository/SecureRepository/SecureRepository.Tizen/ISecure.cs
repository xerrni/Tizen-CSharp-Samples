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
 * @brief       This file contains interface for example classes that interacts with Manager
 */

using System.Collections.Generic;

namespace SecureRepository.Tizen
{
    /// <summary>
    /// Interface contains methods that are responsible for connecting with Managers
    /// <para> Manager: KeyManager, DataManager, CertyficateManager, Pkcs12Manager (not shown in this tuturial example)
    /// </para>
    /// </summary>
    /// <typeparam name="T">Item type (Key,Certificate,Data {byte[]})</typeparam>
    interface ISecure<T>
    {
        //TODO: (For developers) change deafult parameter values in interface implementation 
        //Specially in Keys Class
        /// <summary>
        /// Checks if item Exists in Manager
        /// </summary>
        /// <param name="alias">The name of an item.</param>
        /// <param name="password">The password used to decrypt an item. If password is provided when adding an item to Manager, the same password should be passed here.</param>
        /// <returns>true if Exists, false if not or if encured any errors</returns>
        bool Exists(string alias, string password = null);
        /// <summary>
        /// Gets item from Manager
        /// </summary>
        /// <param name="alias">The name of an item.</param>
        /// <param name="password">The password used to decrypt an item. If password is provided when adding an item to Manager, the same password should be passed here.</param>
        /// <returns>Item if exists or null</returns>
        T Get(string alias, string password);
        /// <summary>
        /// Gets all aviable aliases from Manager
        /// </summary>
        /// <returns>Aliases List or null if aliases do not exist</returns>
        IEnumerable<string> GetAliases();
        /// <summary>
        /// Gets short version of all aviable aliases from Manager
        /// </summary>
        /// <returns>Aliases List or null if aliases do not exist</returns>
        IEnumerable<string> GetAliasesShort();
        /// <summary>
        /// Gets first chars of item content no more than charCount
        /// </summary>
        /// <param name="alias">The name of an item.</param>
        /// <param name="password">The password used to decrypt an item. If password is provided when adding an item to Manager, the same password should be passed here.</param>
        /// <param name="charCount">Maximum chars count that will be returned.</param>
        /// <returns>Item content prefix or null if does not exist</returns>
        string GetContentPrefix(string alias, string password, int charCount = 40);
        /// <summary>
        /// Gets item type
        /// </summary>
        /// <param name="alias">The name of an item.</param>
        /// <param name="password">The password used to decrypt an item. If password is provided when adding an item to Manager, the same password should be passed here.</param>
        /// <returns>Item type if item exists or null</returns>
        string GetType(string alias, string password);
        /// <summary>
        /// Removes item stored inside Manager under specyfied alias
        /// </summary>
        /// <param name="alias">The name of an item.</param>
        void Remove(string alias);
        /// <summary>
        /// Removes all items from Manager
        /// </summary>
        void RemoveAll();
    }
}
