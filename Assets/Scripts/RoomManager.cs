using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
	public List<Room> Rooms;

	public Room GetUnoccupiedRoom()
	{
		var emptyRooms = Rooms.Where<Room>(r => r.IsOccupied == false);
		if (emptyRooms.Count() > 0) return emptyRooms.ElementAtOrDefault(Random.Range(0, emptyRooms.Count()));
		return null;
	}
}
