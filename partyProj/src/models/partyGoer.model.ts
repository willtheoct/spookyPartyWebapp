import { currencies, currency } from "./currency.model";

export enum playerIds {
    wizard,
    hargar,
    playerName,
    overlord,
    adventurer,
    asdf,
    zxcv,
    qwer
}
export class PartyGoer {
    characterName: string;
    level = 1;
    id: playerIds;
    inventory: number[] = Object.values(currencies).slice(Object.entries(currencies).length / 2).map(x => 0);

    static allPlayers = Object.keys(playerIds).slice(Object.entries(playerIds).length / 2);
    // static onlinePlayers: PartyGoer[] = [];
    static onlinePlayers: PartyGoer[] = [];

    /*PartyGoer.allPlayers.map(x => {
    let p = new PartyGoer();
    p.characterName = x;
    p.id = playerIds[x];
    p.level = 1;
    return p;
});
*/
}