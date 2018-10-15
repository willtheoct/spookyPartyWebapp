export enum currencies {
    bronze,
    silver,
    gold,
    platinum,
    trinkets,
    crystals,
    starbucks,
    fudgeTokens,
    keys,
    ironOre,
    rupees
}

export class currency {
    type: currencies;
    count = 0;
}