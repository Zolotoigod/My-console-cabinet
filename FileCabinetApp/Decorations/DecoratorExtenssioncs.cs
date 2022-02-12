using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Decorations
{
    public static class DecoratorExtenssioncs
    {
        public static IFileCabinetService SetTimer(this IFileCabinetService service, bool switcher)
        {
            if (switcher)
            {
                return new ServiceMeter(service);
            }
            else
            {
                return service;
            }
        }

        public static IFileCabinetService SetLogger(this IFileCabinetService service, bool switcher)
        {
            if (switcher)
            {
                return new ServiceLogger(service);
            }
            else
            {
                return service;
            }
        }
    }
}
