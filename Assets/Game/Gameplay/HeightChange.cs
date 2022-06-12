namespace Funzilla
{
	public class HeightChange : Interactable
	{
		internal void Init(Level level, EditHeight info, float position)
		{
			Begin = position - 0.1f;
			End = position + info.spacing;
			level.ChangeHeight(info.height, position);
		}
	}
}