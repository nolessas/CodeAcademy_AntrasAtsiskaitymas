using newConsoleApp2.Models;

namespace newConsoleApp2.Interfaces
{
    public interface IFileHandler
    {
        List<MenuItem> ReadMenuItems();
        void WriteMenuItems(List<MenuItem> menuItems);
        void WriteOrderData(Order order);
        List<Order> ReadOrderData(ITableManager tableManager);
    }
}
