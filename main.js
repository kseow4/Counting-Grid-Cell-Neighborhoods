



const H = 11;
const W = 11;
const N = 3;

const SAMPLE = `░ ▒ ▓ █ ⧆ ⧈ □ ■ ⬚ ⬛ ⬜ ◼ ⯐ ☐ ☒ ☑ ⏹ ⯀ 〼 ▩ ⛋ ﹎ ﹊`;

// const HIT = `█`;
// const HIT = `◼`;
const HIT = `▩`;
const NEIGHBOR = `☒`;
const MISS = `☐`;

const VBP = `║`;
const TLC = `╔`;
const BLC = `╚`;
const HBP = `═`;
const TRC = `╗`;
const BRC = `╝`;

 
class Cell {
   constructor(_value = 0, _x = 0, _y = 0, _isNeighbor = false) {
      this.value = _value;
      this.sign 
      this.positive = _value >= 0;
      this.x = _x;
      this.y = _y;
      this.isNeighbor = _isNeighbor;
   }

   ManhattanDistance = (cell) => Math.abs(this.y - cell.y) + Math.abs(this.x - cell.x);


}

/**
 * Calculates the Manhattan Distance value between two cell coordinates.
 * 
 * @param {Cell} from 
 * @param {Cell} to 
 * @returns {number} The sum of the differences in the two dimensions (Y, X) from the given Cells.
 */
const ManhattanDistance = (from, to) => Math.abs(from.y - from.y) + Math.abs(from.x - to.x);

/**
 * Creates a 2D Array of Cell objects given a 2D Array of numbers.
 * 
 * @param {number[][]} matrix A 2D Array of numeric values. 
 * @returns {Cell[][]} A 2D Array of Cell objects.
 */
function makeCells(matrix) {
   try {
      let moc = [];
      let same_length = matrix[0].length;
      matrix.forEach((row, y) => { 
         if (same_length != row.length) throw `Invalid Matrix!\nRow [${matrix.indexOf(row)}] is inconsistent in length.`;
         let roc = [];
         row.forEach((cell, x) => {
            roc.push(new Cell(cell, x, y));
         });
         moc.push(roc);
      });
      return moc;
   } catch (e) { console.error(e); }
}

/**
 * Generates a random number from specified range.
 * 
 * @param {number} min
 * @param {number} max 
 * @returns {number}
 */
const RandomValueGenerator = (min = Number.MIN_SAFE_INTEGER, max = Number.MAX_SAFE_INTEGER) => Math.random() * (max - min) + min;

/**
 * 
 * @param {number} height 
 * @param {number} width 
 * @returns {}
 */
const mGen = (height, width) => { 
   try {
      let matrix = [];
      for (let y = 0; y < height; y++) {
         let row = [];
         for (let x = 0; x < width; x++) {
            row.push(new Cell(RandomValueGenerator(), x, y));
         }
         matrix.push(row);
      }
      return matrix; 
   } catch (e) { console.error(e); }
};




class Matrix {
   constructor({ h, w, matrix } = { h: H, w: W, matrix: null }) {  
      try {
         if (!matrix instanceof Array) throw ``;

      } catch (e) { console.error(e); }

      this.grid = matrix ?? mGen(h, w); 
      this.height = this.grid.length;
      this.width =  this.grid[0].length;  
      
   };

   findNeighborsWrapping = (cell, n = N) => {
     
      let neighborCount = 0;
      let boundaryY = { min: cell.y - n < 0 ? 0 : cell.y - n, max: cell.y + n > this.height ? this.height : cell.y + n };
      let boundaryX = { min: cell.x - n < 0 ? 0 : cell.x - n, max: cell.x + n > this.width ? this.width : cell.x + n };
      
      let ymin = this.height + cell.y + 1

      for (let y = boundaryY.min; y <= boundaryY.max; y++) {
         for (let x = boundaryX.min; x <= boundaryX.max; x++) {
            let neighborCell = this.grid[y][x];

       
         }
      }


   };

   findNeighbors = (cell, n = N) => {
      
      let neighborCount = 0;
      let boundaryY = { min: cell.y - n < 0 ? 0 : cell.y - n, max: cell.y + n > this.height ? this.height : cell.y + n };
      let boundaryX = { min: cell.x - n < 0 ? 0 : cell.x - n, max: cell.x + n > this.width ? this.width : cell.x + n };
      
      for (let y = boundaryY.min; y <= boundaryY.max; y++) {

         for (let x = boundaryX.min; x <= boundaryX.max; x++) {
            let neighborCell = this.grid[y][x];
            if (cell.ManhattanDistance(neighborCell) <= n && !neighborCell.isNeighbor) {
               // if (!neighborCell.isNeighbor) {
                  neighborCell.isNeighbor = true;
                  neighborCount++;
               // }
            }
         }
      }
      return neighborCount;
   };
    
   analyzeGrid = () => {
      let positiveCells = []; 
      let neighborhood = 0; 
      
      
      for (let y = 0; y < this.height; y++) {
         for (let x = 0; x < this.width; x++) {
            let cell = this.grid[y][x];
            if (cell.positive) { 
               cell.isNeighbor = true;
               positiveCells.push(cell);
               neighborhood++;
               neighborhood += this.findNeighbors(cell);
            }
         } 
      }

      console.log("Positive Cells:", positiveCells);
      // console.log(print);
      console.log("# of cells in the Neighborhood:", neighborhood);

   }

   printGrid = () => {
      let output = `${TLC}${HBP.repeat(this.width * 2 + 2)}${TRC}`
      for (let y = 0; y < this.height; y++) {
         let row = `\n${VBP}`; 
         for (let x = 0; x < this.width; x++) {
            row += ` ${this.grid[y][x].positive ? HIT : this.grid[y][x].isNeighbor ? NEIGHBOR : MISS}`;
         }
         // ret += `${row} ║\n${VBP}${" ".repeat(this.width * 2 + 1)}${VBP}`;
         output += `${row}${" ".repeat(2)}${VBP}`;
      }
      output += `\n${BLC}${HBP.repeat(this.width * 2 + 2)}${BRC}`;
      return output;
   };

   printCells = () => {
      let output = ``;
      for (let y = 0; y < this.height; y++) {
         let row = `\n`; 
         for (let x = 0; x < this.width; x++) {
            row += `${this.grid[y][x].value} `;
         } 
         output += row;
      }
      return output;
   }
    
}


const test1 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test2 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test3 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, 1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test4 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test5 = Object.freeze(makeCells([[1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));



console.log("CELL:", new Cell(-1, 0,0))
// console.log("TEST:", test4)
let m = new Matrix({matrix:test5}); 
m.analyzeGrid();
console.log(m.printGrid()); 




















