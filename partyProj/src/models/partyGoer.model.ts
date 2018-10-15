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
    inventory: currency[] = [];

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