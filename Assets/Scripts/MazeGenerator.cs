using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject maze_1;
    public GameObject maze_2;
    public GameObject maze_3;
    public GameObject maze_4;
    public GameObject player;
    private GameObject first_maze;
    private GameObject second_maze;
    private float offset_z_1 = 92.6f;
    private float offset_z_2 = 80.1f;
    private int[] maze_indexes = { -1, -1, -1, -1 };
    private float first_pos = -6.5f;
    private float start_position;
    private int index;
    private int new_index;

    void Start()
    {
        start_position = player.transform.position.z;
        maze_indexes[0] = 0;
        index = 0;
        new_index = 0;
        Vector3 position = new Vector3(-0.8f, 0f, first_pos);
        first_maze = Instantiate(maze_1, position, Quaternion.Euler(0, 0, 0));
        second_maze = GenerateRandomMaze(ref maze_indexes, ref first_pos, ref new_index);
    }

    private GameObject GenerateRandomMaze(ref int[] maze_indexes, ref float first_pos, ref int new_index)
    {
        int random_index = Random.Range(0, 4);
        int count = 0;
        int i = 0;
        while (i != 4)
        {
            if (maze_indexes[i] != -1)
            {
                count += 1;
            }
            if (random_index == maze_indexes[i])
            {
                random_index = Random.Range(0, 4);
                i = 0;
                count = 0;
            }
            else
            {
                i += 1;
            }
        }
        maze_indexes[count + 1] = random_index;
        new_index = random_index;
        if (random_index == 1 || random_index == 2)
        {
            Vector3 position = new Vector3(-0.8f, -0.75f, first_pos - offset_z_1);
            first_pos -= offset_z_1;
            if (random_index == 1)
            {
                return Instantiate(maze_2, position, Quaternion.Euler(0, 0, 90));
            }
            else
            {
                return Instantiate(maze_3, position, Quaternion.Euler(0, 0, 90));
            }
        }
        else
        {
            Vector3 position = new Vector3(-0.8f, 0f, first_pos - offset_z_2);
            first_pos -= offset_z_2;
            if (random_index == 0)
            {
                return Instantiate(maze_1, position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                return Instantiate(maze_4, position, Quaternion.Euler(0, 0, 0));
            }
        }
    }

    void Update()
    {
        if (index == 0 || index == 3)
        {
            if (start_position - player.transform.position.z >= offset_z_2)
            {
                if (first_maze is not null)
                {
                    if (maze_indexes[3] != -1)
                    {
                        maze_indexes[0] = new_index;
                        for (int i = 1; i < 4; ++i)
                        {
                            maze_indexes[i] = -1;
                        }
                    }
                    Destroy(first_maze);
                    start_position -= offset_z_2;
                    first_maze = second_maze;
                    index = new_index;
                    second_maze = GenerateRandomMaze(ref maze_indexes, ref first_pos, ref new_index);

                    // Пополнение гранат
                    RefillGrenades();
                }
            }
        }
        else
        {
            if (start_position - player.transform.position.z >= offset_z_1)
            {
                if (first_maze is not null)
                {
                    if (maze_indexes[3] != -1)
                    {
                        maze_indexes[0] = new_index;
                        for (int i = 1; i < 4; ++i)
                        {
                            maze_indexes[i] = -1;
                        }
                    }
                    Destroy(first_maze);
                    start_position -= offset_z_1;
                    first_maze = second_maze;
                    index = new_index;
                    second_maze = GenerateRandomMaze(ref maze_indexes, ref first_pos, ref new_index);

                    // Пополнение гранат
                    RefillGrenades();
                }
            }
        }
    }

    private void RefillGrenades()
    {
        GrenadeWeapon grenadeWeapon = player.GetComponent<GrenadeWeapon>();
        if (grenadeWeapon != null)
        {
            grenadeWeapon.RefillGrenades();
        }
    }
}
