using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
	public List<Room> Rooms;

	private void Update()
	{
		// Vacate rooms with recovered and dead patients if the player is not in the room
		foreach (var room in Rooms)
		{
			if (room.IsOccupied && !room.IsPlayerInRoom)
			{
				if (room.Patient.Hearts == 0 || room.Patient.Hearts == Patient.MaxHearts)
				{
					// Disable the patient object
					room.Patient.gameObject.SetActive(false);

					// Unregister the patient from the room
					room.Patient = null;
				}
			}
		}
	}

	public Room GetUnoccupiedRoom()
	{
		var emptyRooms = Rooms.Where<Room>(r => r.IsOccupied == false);
		if (emptyRooms.Count() > 0) return emptyRooms.ElementAtOrDefault(Random.Range(0, emptyRooms.Count()));
		return null;
	}
}
