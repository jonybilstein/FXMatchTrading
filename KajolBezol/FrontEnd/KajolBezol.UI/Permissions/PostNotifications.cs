using Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace KajolBezol.UI.Permissions
{
    public class PostNotifications : BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
                    new (string, bool)[] { (Manifest.Permission.PostNotifications, true) };
    }
}
