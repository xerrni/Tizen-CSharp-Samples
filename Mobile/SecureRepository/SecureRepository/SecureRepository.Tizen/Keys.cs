﻿/*
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
 * @brief       This file contains example usage of Key, KeyManager class
 */

namespace SecureRepository.Tizen
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using global::Tizen;
    using global::Tizen.Security.SecureRepository;

    /// <summary>
    /// Example usage of Key and KeyManager class.
    /// </summary>
    public class Keys : Secure<Key>
    {
        #region Implement Interface

        /// <summary>
        /// Gets Key saved under specified alias from KeyManager.
        /// </summary>
        /// <param name="alias">The name of a Key.</param>
        /// <param name="password">The password used to decrypt a Key. If password is provided when adding a Key to KeyManager, the same password should be passed here.</param>
        /// <returns>Key if exists, else null.</returns>
        public override Key Get(string alias, string password)
        {
            Key data = null;
            if (this.Exists(alias, password))
            {
                try
                {
                    data = KeyManager.Get(alias, password);
                    return data;
                }
                catch (Exception ex)
                {
                    Log.Error("SecureRepository_KEYS", ex.Message);
                }
            }
            else
            {
                Log.Error("SecureRepository_KEYS", "No Keys stored under this alias");
            }

            return null;
        }

        /// <summary>
        /// Gets all available aliases from KeyManager.
        /// </summary>
        /// <returns>Aliases List if exists, else null.</returns>
        public override IEnumerable<string> GetAliases()
        {
            IEnumerable<string> aliases;
            try
            {
                aliases = KeyManager.GetAliases();
            }
            catch
            {
                return null;
            }

            return aliases;
        }

        /// <summary>
        /// Gets first chars of Key content no more than charCount.
        /// </summary>
        /// <param name="alias">The name of a Key.</param>
        /// <param name="password">The password used to decrypt a Key. If password is provided when adding a Key to KeyManager, the same password should be passed here.</param>
        /// <param name="charCount">Maximum chars count that will be returned.</param>
        /// <returns>Key content prefix if exists, else null.</returns>
        public override string GetContentPrefix(string alias, string password = null, int charCount = 80)
        {
            string prefix = null;
            Key k = this.Get(alias, password);

            if (k != null)
            {
                prefix = Cryptography.ByteArrayToHexViaLookup32(k.Binary);
                if (prefix.Length > charCount)
                {
                    prefix = prefix.Substring(0, charCount);
                }

                return prefix;
            }

            return null;
        }

        /// <summary>
        /// Gets Key type.
        /// </summary>
        /// <param name="alias">The name of a Key.</param>
        /// <param name="password">The password used to decrypt a Key. If password is provided when adding a Key to KeyManager, the same password should be passed here.</param>
        /// <returns>Key Type if Key exists, else null.</returns>
        public override string GetType(string alias, string password = null)
        {
            Key k = this.Get(alias, password);

            if (k != null)
            {
                return k.Type.ToString();
            }
            else
            {
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Adds Aes Key to KeyManager if you pass null as second parameter the Key will be generated automatically.
        /// </summary>
        /// <param name="alias">The name of a Key.</param>
        /// <param name="key">The binary content of Key.</param>
        /// <param name="password">The password that will be used to encrypt an Key value.</param>
        public void AddAesKey(string alias, byte[] key = null, string password = null)
        {
            if (key == null)
            {
                if (!this.Exists(alias, password))
                {
                    KeyManager.CreateAesKey(256, alias, new Policy(password, true)); // Creates new Aes key if it`s not already inside KeyManager.
                }
            }
            else
            {
                Key k = new Key(key, KeyType.Aes, password); // Creates Key (generic class for holding Keys e.g. Aes,Rsa.
                try
                {
                    if (!this.Exists(alias, password))
                    {
                        KeyManager.Save(alias, k, new Policy(password, true)); // Saves Created key to KeyManager.
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("SecureRepository_KEYS", ex.Message);
                }
            }
        }

        /// <summary>
        /// Adds Rsa private Key to KeyManager.
        /// </summary>
        /// <param name="alias">The name of a Key.</param>
        /// <param name="privateKeyString">The binary content of Key.</param>
        public void AddPrivateRsaKey(string alias, string privateKeyString = null)
        {
            string password = null; // "MY_SECURE_PASSWORD_FOR_RSA_PRIVATE_KEY";

            // replace it with your own Key.
            if (privateKeyString == null)
            {
                privateKeyString = "-----BEGIN RSA PRIVATE KEY-----\n" +
                                   "MIICXAIBAAKBgQCqGKukO1De7zhZj6+H0qtjTkVxwTCpvKe4eCZ0FPqri0cb2JZfXJ/DgYSF6vUp\n" +
                                   "wmJG8wVQZKjeGcjDOL5UlsuusFncCzWBQ7RKNUSesmQRMSGkVb1/3j+skZ6UtW+5u09lHNsj6tQ5\n" +
                                   "1s1SPrCBkedbNf0Tp0GbMJDyR4e9T04ZZwIDAQABAoGAFijko56+qGyN8M0RVyaRAXz++xTqHBLh\n" +
                                   "3tx4VgMtrQ+WEgCjhoTwo23KMBAuJGSYnRmoBZM3lMfTKevIkAidPExvYCdm5dYq3XToLkkLv5L2\n" +
                                   "pIIVOFMDG+KESnAFV7l2c+cnzRMW0+b6f8mR1CJzZuxVLL6Q02fvLi55/mbSYxECQQDeAw6fiIQX\n" +
                                   "GukBI4eMZZt4nscy2o12KyYner3VpoeE+Np2q+Z3pvAMd/aNzQ/W9WaI+NRfcxUJrmfPwIGm63il\n" +
                                   "AkEAxCL5HQb2bQr4ByorcMWm/hEP2MZzROV73yF41hPsRC9m66KrheO9HPTJuo3/9s5p+sqGxOlF\n" +
                                   "L0NDt4SkosjgGwJAFklyR1uZ/wPJjj611cdBcztlPdqoxssQGnh85BzCj/u3WqBpE2vjvyyvyI5k\n" +
                                   "X6zk7S0ljKtt2jny2+00VsBerQJBAJGC1Mg5Oydo5NwD6BiROrPxGo2bpTbu/fhrT8ebHkTz2epl\n" +
                                   "U9VQQSQzY1oZMVX8i1m5WUTLPz2yLJIBQVdXqhMCQBGoiuSoSjafUhV7i1cEGpb88h5NBYZzWXGZ\n" +
                                   "37sJ5QsW+sJyoNde3xH8vdXhzU7eT82D6X/scw9RZz+/6rCJ4p0=\n" +
                                   "-----END RSA PRIVATE KEY-----";
            }

            Key key = new Key(Encoding.UTF8.GetBytes(privateKeyString), KeyType.RsaPrivate, password); // Creates new Rsa private Key.
            try
            {
                if (!this.Exists(alias, password))
                {
                    KeyManager.Save(alias, key, new Policy(password, true)); // Saves Created Key to KeyManager.
                }
            }
            catch (Exception ex)
            {
                Log.Error("SecureRepository_KEYS", ex.Message);
            }
        }

        /// <summary>
        /// Adds Rsa public Key to KeyManager.
        /// </summary>
        /// <param name="alias">The name of a Key.</param>
        /// <param name="publicKeyString">The binary content of Key.</param>
        public void AddPublicRsaKey(string alias, string publicKeyString = null) // Add public Rsa key to KeyManager.
        {
            string password = null; // "MY_SECURE_PASSWORD_FOR_RSA_PUBLIC_KEY"; //You can define it here or edit method to take it as a paramether.

            // replace it with your own key.
            if (publicKeyString == null)
            {
                publicKeyString = "-----BEGIN PUBLIC KEY-----\n" +
               "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2b1bXDa+S8/MGWnMkru4\n" +
               "T4tUddtZNi0NVjQn9RFH1NMa220GsRhRO56F77FlSVFKfSfVZKIiWg6C+DVCkcLf\n" +
               "zXJ/Z0pvwOQYBAqVMFjV6efQGN0JzJ1Unu7pPRiZl7RKGEI+cyzzrcDyrLLrQ2W7\n" +
               "0ZySkNEOv6Frx9JgC5NExuYY4lk2fQQa38JXiZkfyzif2em0px7mXbyf5LjccsKq\n" +
               "v1e+XLtMsL0ZefRcqsP++NzQAI8fKX7WBT+qK0HJDLiHrKOTWYzx6CwJ66LD/vvf\n" +
               "j55xtsKDLVDbsotvf8/m6VLMab+vqKk11TP4tq6yo0mwyTADvgl1zowQEO9I1W6o\n" +
               "zQIDAQAB\n" +
               "-----END PUBLIC KEY-----\n";
            }

            Key key = new Key(Encoding.UTF8.GetBytes(publicKeyString), KeyType.RsaPublic, password);
            try
            {
                if (!this.Exists(alias, password))
                {
                    KeyManager.Save(alias, key, new Policy(password, true));
                }
            }
            catch (Exception ex)
            {
                Log.Error("SecureRepository_KEYS", ex.Message);
            }
        }

        /// <summary>
        /// Creates Rsa Key pair and saves it inside KeyManager.
        /// </summary>
        /// <param name="privateKeyAlias">The name of a private Key.</param>
        /// <param name="publicKeyAlias">The name of a public Key.</param>
        public void CreateRsaKeyPair(string privateKeyAlias, string publicKeyAlias) // You can extend this method to support password protection.
        {
            if (publicKeyAlias == null || privateKeyAlias == null)
            {
                throw new System.ArgumentException("Argument is \"null\"");
            }

            // Checks if Keys exist in KeyManager under this aliases.
            if (!(this.Exists(privateKeyAlias) && !this.Exists(publicKeyAlias)))
            {
                try
                {
                    KeyManager.CreateRsaKeyPair(1024, privateKeyAlias, publicKeyAlias, new Policy(), new Policy()); // USE 4096 in real application!!!!, lowered to speed up.
                }
                catch (Exception ex)
                {
                    Log.Error("SecureRepository_KEYS", ex.Message);
                }
            }
            else
            {
                Log.Error("SecureRepository_KEYS", "Alias already in use.");
            }
        }
    }
}
