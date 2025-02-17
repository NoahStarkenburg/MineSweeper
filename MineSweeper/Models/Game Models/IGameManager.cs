namespace MineSweeper.Models.Game_Models
{
    public interface IGameManager
    {
        public List<ScoresModel> GetAllSores();
        public List<ScoresModel> GetScoreById();
        public List<ScoresModel> GetScoreByUser();
        public int AddScore(ScoresModel score);
        public void UpdateScore(ScoresModel score);
        public void DeleteScore(ScoresModel score);
    }
}
