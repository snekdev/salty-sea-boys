using UnityEngine;
using System.Collections;
/*
 * @author SoftRare - www.softrare.eu
 * This is a uMMO enum.
 * You may only use and change this code if you purchased it in a legal way: From the official Unity Asset Store or directly from the author SoftRare.
 * Please read the in-Editor documentation for further information on how to use the code of this plugin.
 */
namespace SoftRare.Net {
    public enum ObjectType {
        Player, //a real network player
        NonPlayerObject //NPCs and other non-player-objects like buildings/coins/everything which should be syncronized across the network
    }
}
