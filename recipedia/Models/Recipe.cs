namespace recipedia.Models
{
    public class Recipe
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string ImageURL { set; get; }
        public string Meal { set; get; }
        public string Cuisine { set; get; }
        public string Diet { set; get; }
        public string Ingredients { set; get; }
        public int Rating { set; get; }
        public string Time { set; get; }

    }
}
