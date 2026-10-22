using System;

namespace Фитнес
{
    public static class SubscriptionsHelper
    {
        public static void ОбновитьСтатусы()
        {
            try
            {
                var today = DateTime.Today;

                DbHelper.Execute(
                    "UPDATE [Абонементы] SET [статус] = 'неактивен' WHERE [конец] < ?",
                    today);

                DbHelper.Execute(
                    "UPDATE [Абонементы] SET [статус] = 'активно' WHERE [конец] >= ?",
                    today);
            }
            catch
            {
            }
        }
    }
}