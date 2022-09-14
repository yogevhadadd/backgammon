using Game.model;

namespace Game.Service
{
    public class GameService
    {
        private Location[] locations = new Location[24];


        public Location[] PlayTurn(Turn[] turns)
        {
            for(int i = 0; i < turns.Length; i++)
            {
                if (turns[0].color == "white")
                {
                    locations[turns[i].From].Amount--;
                    locations[turns[i].From + turns[i].Amount].Amount++;
                }
                else
                {
                    locations[turns[i].From].Amount--;
                    locations[turns[i].From - turns[i].Amount].Amount++;
                }
                
            }

            return locations;
        }

    }
}
