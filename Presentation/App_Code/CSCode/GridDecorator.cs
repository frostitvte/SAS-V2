using System.Web.UI.WebControls;

public class GridDecorator
{
    public static void MergeRows(GridView gridView)
    {
        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            for (int cellIndex = 0; cellIndex < row.Cells.Count; cellIndex++)
            {
                string text = ((Label)row.Cells[cellIndex].FindControl("Llabel" + cellIndex)).Text;
                string previousText = ((Label)previousRow.Cells[cellIndex].FindControl("Llabel" + cellIndex)).Text;

                if (text == previousText)
                {
                    row.Cells[cellIndex].RowSpan = previousRow.Cells[cellIndex].RowSpan < 2 ? 2 : previousRow.Cells[cellIndex].RowSpan + 1;
                    previousRow.Cells[cellIndex].Visible = false;
                }
            }
        }
    }
}
