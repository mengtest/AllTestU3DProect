using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Main
{
    [System.Serializable]
    public enum RoleType
    {
        /// <summary>
        /// 无角色,未选地主前
        /// </summary>
        noRole,
        /// <summary>
        /// 农民
        /// </summary>
        peasant,
        /// <summary>
        /// 地主
        /// </summary>
        landlord,
    }
}
