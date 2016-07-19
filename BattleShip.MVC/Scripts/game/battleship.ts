/**
 * Describes Board Position for Elements
 */
class BoardPosition {
    private xPosition: number;
    private yPosition: number;

    setXPosition(xValue : number) {
        this.xPosition = xValue;
    }

    getXPosition() : number {
        return this.xPosition;
    }

    setYPosition(yValue : number) {
        this.yPosition = yValue;
    }

    getYPosition() : number {
        return this.yPosition;
    }

    setCoordinates(xValue : number, yValue: number) {
        this.xPosition = xValue;
        this.yPosition = yValue;
    }
}

class Ship {
    private lifes: number;
    private position: BoardPosition;
    private isOnXAxis: boolean;
    private name: string;
    private hasBeenSet: boolean;
    private size: number;
    private element: HTMLElement;

    constructor(lifes, name) {
        this.lifes = lifes;
        this.name = name;
        this.size = lifes;
    }

    getLifes() {
        return this.lifes;
    }

    setLifes(lifes : number) {
        this.lifes = lifes;
    }

    getPosition() : BoardPosition {
        return this.position;
    }

    setPosition(position: BoardPosition) {
        this.position = position;
    }

    getIsOnXAxis() : boolean{
        return this.isOnXAxis;
    }

    setIsOnXAxis(isOnXAxis: boolean) {
        this.isOnXAxis = isOnXAxis;
    }

    getHasBeenSet() : boolean {
        return this.hasBeenSet;
    }

    setHasBeenSet(hasBeenSet: boolean) {
        this.hasBeenSet = hasBeenSet;
    }

    getSize() : number {
        return this.size;
    }

    setSize(size: number) {
        this.size = size;
    }
}

class Game {
    GAMEBOARD: HTMLElement = document.getElementById('gameboard');
    DOCKS: HTMLElement = document.getElementById('docks');
    game;
    ships: Array<Ship>;

    setMap() {
        
    }

    startGame() {

    }

    setShips(ships: Array<Ship>) { }

    genCellId(j: number, i: number) { }

    createGame() { }

    buildBoard() { }

    buildFireBoard() { }

    buildShip() { }

    loadShips() { }

    rotate() { }

    drag_start() { }

    drag_over() { }

    drag_leave() { }

    drag_enter() { }

    drop() { }

    setShip() { }


}