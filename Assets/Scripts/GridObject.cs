
namespace Wordle
{
    public class GridObject<T>
    {
        GridSystem2D<GridObject<T>> grid;
        public int x;
        public int y;

        T letter;

        public GridObject(GridSystem2D<GridObject<T>> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetValue(T letter) => this.letter = letter;
        public T GetValue() => letter;
    }
}