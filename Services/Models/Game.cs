namespace Services.Models
{

    public class GameCreateRequest
    {
        public string Name { get; set; }
    }
    public class GameUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class GameDeleteRequest
    {
        public int Id { get; set; }
    }

    public class GameGetByRequest
    {
        public int Id { get; set; }
    }
    public class GameGetByResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class GameGetAllResponse
    {
        public ICollection<GameGetAllModel> Games { get; set; } = [];
    }
    public class GameGetAllModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class GameDisplayCreateResponse
    {
        public string Name { get; set; }
    }

    public class GameDisplayUpdateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GameDisplayDeleteResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GameDisplayUpdateRequest
    {
        public int Id { get; set; }
    }

    public class GameDisplayDeleteRequest
    {
        public int Id { get; set; }
    }

}
