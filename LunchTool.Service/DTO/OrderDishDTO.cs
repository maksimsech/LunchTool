namespace LunchTool.Service.DTO
{
    class OrderDishDTO
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int OrderId { get; set; }

        public int DishId { get; set; }
    }
}
