using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace Funzilla
{
	internal class Level : EditObject
	{
		internal Path Path { get; private set; }
		internal float Length { get; private set; }
		private Vector2 _point;
		private Vector2 _direction = Vector2.up;
		internal int Height { get; private set; }
		private bool _finished;

		private readonly LinkedList<Interactable> _interactables = new LinkedList<Interactable>();
		private readonly List<PathStraight> _straights = new List<PathStraight>(4);
		internal List<EditHole> Holes = new List<EditHole>();

		//add building
		[Header("Reference to edit Building")] 
		[SerializeField] private bool useBuilding = false;
		[SerializeField] private GameObject buildingPref;
		[SerializeField] private int distancePerBuilding = 60;
		[SerializeField] private int leftDistance=45;
		[SerializeField] private int hightOfBuilding = -25; 
		[SerializeField] private int numOfBuilding = 5; 
		private void AddStraight(float position)
		{
			var length = position - Length;
			if (length < 0.01f) return;
			var exit = _point + _direction * length;
			var path = new PathStraight(Length, _point, exit, Height, Holes) {Finishing = _finished};
			Length += path.Length;

			Path?.Append(path);
			Path = path;
			_point = exit;
			Holes = new List<EditHole>();
			_straights.Add(path);
		}

		internal void Init()
		{
			var length = Init(this, transform, 10);
			AddStraight(length);

			foreach (var straight in _straights)
			{
				straight.Spawn(this);
			}

			_finished = true;
			var finish = Gameplay.Instance.Finish;
			finish.Init(this);

			Path = _straights[0];
			Path.Place(finish.transform, length);

			foreach (var interactable in _interactables)
			{
				Path.Place(interactable.transform, interactable.Begin);
			}

			_finished = false;

			if(useBuilding) GenerateBuildings();
			Length = length;
		}

		private void GenerateBuildings()
		{
#if UNITY_EDITOR
			buildingPref =(GameObject) AssetDatabase.LoadAssetAtPath("Assets/Game/Gameplay/Models/Building.prefab", typeof(GameObject) );
			foreach (var straight in _straights)
			{
				if(straight.Length==3) return;
				
				for (int i = 0; i <= numOfBuilding;i++){
					if (straight.Direction.y > 0)
					{
						Instantiate(buildingPref, new Vector3(straight.Entry.x - leftDistance,hightOfBuilding, straight.Entry.y+distancePerBuilding*i),
							Quaternion.identity).transform.parent =gameObject.transform;
						
					}
					else if (straight.Direction.y < 0 )
					{
						if(i==0) continue;
						Instantiate(buildingPref, new Vector3(straight.Entry.x + leftDistance, hightOfBuilding, straight.Entry.y- distancePerBuilding*i),
							Quaternion.identity).transform.parent =gameObject.transform;
					}

					else 
					{
						if(i==0) continue;
						Instantiate(buildingPref, new Vector3(straight.Entry.x+distancePerBuilding*i, hightOfBuilding, straight.Entry.y+leftDistance),
							Quaternion.identity).transform.parent =gameObject.transform;
					}
				}
			}
#endif
		}

		internal void AddFinishStep(Straight straight, int index)
		{
			if (index > 0) Height++;
			AddStraight(Length + 3);
			Path.Place(straight.transform, Length - 3);
			straight.Setup(Gameplay.RoadWidth, 3, Height);
		}

		internal PathTurn Turn(EditTurn turn, float begin)
		{
			AddStraight(begin);
			var directionOut = turn.left
				? new Vector2(-_direction.y, _direction.x)
				: new Vector2(_direction.y, -_direction.x);
			var path = new PathTurn(begin, _direction, directionOut, _point, Height);
			Path?.Append(path);
			Path = path;
			_point = path.Exit;
			_direction = directionOut;
			Length += path.Length;
			return path;
		}

		internal void ChangeHeight(int delta, float begin)
		{
			AddStraight(begin);
			Height += delta;
		}

		internal void AddInteractable(Interactable obj)
		{
			_interactables.AddLast(obj);
		}

		private void OnDisable()
		{
			DOTween.KillAll();
		}
	}
}