import { currencies, currency } from "./currency.model";

export enum playerIds {
    wallace,
    hargar,
    playerName,
    whethers,
    weaver,
    asdf,
    zxcv,
    qwer
}
export class PartyGoer {
    characterName: string;
    level = 1;
    id: playerIds;
    inventory: number[] = Object.values(currencies).slice(Object.entries(currencies).length / 2).map(x => 0);
    passPhrase = "";

    static allPlayers = Object.keys(playerIds).slice(Object.entries(playerIds).length / 2);
    // static onlinePlayers: PartyGoer[] = [];
    static onlinePlayers: PartyGoer[] = [];
}