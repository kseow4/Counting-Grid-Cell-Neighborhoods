



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
   constructor(value = -1, isNeighbor = false) {
      this.value = value;
      this.isNeighbor = isNeighbor;
   }
   get positive() { return this.value >= 0; }
}

/**
 * Calculates the Manhattan Distance value between two cell coordinates.
 * 
 * @param {Cell} from 
 * @param {Cell} to 
 * @returns {number} The sum of the differences in the two dimensions (Y, X) from the given Cells.
 */
const ManhattanDistance = (fromY, fromX, toY, toX) => Math.abs(fromY - toY) + Math.abs(fromX - toX);

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
            // roc.push(new Cell(cell, x, y));
            roc.push(new Cell(cell));
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
function MatrixGenerator(height, width, positives) { 
   try {
      let matrix = Array.from({ length: height }, () => (Array.from({ length: width }, () => positives ? new Cell() : new Cell(RandomValueGenerator()))));

      switch(typeof positives) {
         case 'object':
            if (Array.isArray(positives)) { 
               positives.forEach(cell => { 
                  if (cell.y > height || cell.y < 0 || !Number.isInteger(cell.y)) throw `Cell (${cell.y}, ${cell.x}) has an invalid Y value: ${cell.y}.`;
                  if (cell.x > width || cell.x < 0 || !Number.isInteger(cell.x)) throw `Cell (${cell.y}, ${cell.x}) has an invalid X value: ${cell.x}.`;
                  matrix[cell.y][cell.x].value = Math.abs(matrix[cell.y][cell.x].value);
               });
            }
            break;

         case 'number': 
            if (!Number.isInteger(positives)) throw `Invalid number of positive cells specified: ${positives}.`;
            let used = [];
            for (let i = 0; i < positives; i++) {           
               while (true) {
                  var y = Math.floor(Math.random() * height);
                  var x = Math.floor(Math.random() * width);
                  if (!used.find(cell => cell.y == y && cell.x == x)) { 
                     used.push({ y, x });
                     break;
                  }
               }
               matrix[y][x].value = Math.abs(matrix[y][x].value);
            }
            break;

         default:
            throw `Invalid input type [${typeof positives}]: ${positives}.`;

      }

      return matrix; 
   } catch (e) { console.error(e); }
};




// class Matrix {
//    constructor({ h, w, matrix } = { h: H, w: W, matrix: null }) {  
//       try {
//          if (!matrix instanceof Array) throw ``;

//       } catch (e) { console.error(e); }

//       this.grid = matrix ?? MatrixGenerator(h, w); 
//       this.height = this.grid.length;
//       this.width =  this.grid[0].length;  
      
//    };

//    findNeighborsWrapping = (cell, n = N) => {
     
//       let neighborCount = 0;
//       let boundaryY = { min: cell.y - n < 0 ? 0 : cell.y - n, max: cell.y + n > this.height ? this.height : cell.y + n };
//       let boundaryX = { min: cell.x - n < 0 ? 0 : cell.x - n, max: cell.x + n > this.width ? this.width : cell.x + n };
      
//       let ymin = this.height + cell.y + 1

//       for (let y = boundaryY.min; y <= boundaryY.max; y++) {
//          for (let x = boundaryX.min; x <= boundaryX.max; x++) {
//             let neighborCell = this.grid[y][x];

       
//          }
//       }


//    };

//    findNeighbors = (matrix, y, x, n) => {
      
//       let neighborCount = 0;
//       let boundaryY = { min: y - n < 0 ? 0 : y - n, max: y + n > this.height ? this.height : y + n };
//       let boundaryX = { min: x - n < 0 ? 0 : x - n, max: x + n > this.width ? this.width : x + n };
      


//       for (let nY = boundaryY.min; nY <= boundaryY.max; nY++) {
//          for (let nX = boundaryX.min; nX <= boundaryX.max; nX++) {
//             let neighborCell = this.grid[nY][nX];
//             if (ManhattanDistance(y, x, nY, nX) <= n && !neighborCell.isNeighbor) {
//                neighborCell.isNeighbor = true;
//                neighborCount++;
//             }
//          }
//       }
//       return neighborCount;
//    };
   
//    analyzeGrid = (matrix, n = N) => {
//       let positiveCells = []; 
//       let neighborhood = 0; 

//       const find = (y, x) => {
//          let neighborCount = 0;
//          let boundaryY = { min: y - n < 0 ? 0 : y - n, max: y + n > matrix.length ? matrix.length : y + n };
//          // matrix.slice(boundaryY.min, boundaryY.max + 1).forEach((row, y2) => {
//          //    let boundaryX = { min: x - n < 0 ? 0 : x - n, max: x + n > row.length ? row.length : x + n };
//          //    row.slice(boundaryX.min, boundaryX.max + 1).forEach((cell, x2) => {
//          //       if (ManhattanDistance(y, x, y2, x2) <= n && !cell.isNeighbor) {
//          //          cell.isNeighbor = true;
//          //          neighborCount++;
//          //       }
//          //    });
//          // });
//          // matrix.slice(boundaryY.min, boundaryY.max + 1).forEach((r,u) => {
//          //    let boundaryX = { min: x - n < 0 ? 0 : x - n, max: x + n > r.length ? r.length : x + n };
//          //    console.log("BB",boundaryY)
//          //    r.slice(boundaryX.min, boundaryX.max + 1).forEach((c, v)=>{
//          //       console.log(v, boundaryX)
//          //    })
//          // });
//          for (let nY = boundaryY.min; nY <= boundaryY.max; nY++) {
//             let boundaryX = { min: x - n < 0 ? 0 : x - n, max: x + n > matrix[nY].length ? matrix[nY].length : x + n };
//             for (let nX = boundaryX.min; nX <= boundaryX.max; nX++) {
//                let neighborCell = matrix[nY][nX];
//                if (ManhattanDistance(y, x, nY, nX) <= n && !neighborCell.isNeighbor) {
//                   neighborCell.isNeighbor = true;
//                   neighborCount++;
//                }
//             }
//          }
//          return neighborCount;
//       }
      
//       matrix.forEach((row, y) => {

//          row.forEach((cell, x) => {
//             if (cell.positive) {
//                cell.isNeighbor = true;
//                positiveCells.push(cell);
//                neighborhood++;
//                neighborhood += find(y, x);
//             }
//          });
//       });
      
//       // for (let y = 0; y < this.height; y++) {
//       //    for (let x = 0; x < this.width; x++) {
//       //       let cell = this.grid[y][x];
//       //       if (cell.positive) { 
//       //          cell.isNeighbor = true;
//       //          positiveCells.push(cell);
//       //          neighborhood++;
//       //          neighborhood += this.findNeighbors(y, x, n);
//       //       }
//       //    } 
//       // }

//       console.log("Positive Cells:", positiveCells);
//       // console.log(print);
//       console.log("# of cells in the Neighborhood:", neighborhood);

//    }

//    printGrid = () => {
//       let output = `${TLC}${HBP.repeat(this.width * 2 + 2)}${TRC}`
//       for (let y = 0; y < this.height; y++) {
//          let row = `\n${VBP}`; 
//          for (let x = 0; x < this.width; x++) {
//             row += ` ${this.grid[y][x].positive ? HIT : this.grid[y][x].isNeighbor ? NEIGHBOR : MISS}`;
//          }
//          // ret += `${row} ║\n${VBP}${" ".repeat(this.width * 2 + 1)}${VBP}`;
//          output += `${row}${" ".repeat(2)}${VBP}`;
//       }
//       output += `\n${BLC}${HBP.repeat(this.width * 2 + 2)}${BRC}`;
//       return output;
//    };

//    printCells = () => {
//       let output = ``;
//       for (let y = 0; y < this.height; y++) {
//          let row = `\n`; 
//          for (let x = 0; x < this.width; x++) {
//             row += `${this.grid[y][x].value} `;
//          } 
//          output += row;
//       }
//       return output;
//    }
    
// }

const analyzeGrid = (matrix, n = N) => {
   let positiveCells = []; 
   let neighborhood = 0; 

   const find = (y, x) => {
      let neighborCount = 0;
      let boundaryY = { min: y - n < 0 ? 0 : y - n, max: y + n >= matrix.length ? matrix.length : y + n + 1};
      // matrix.slice(boundaryY.min, boundaryY.max).forEach((row, y2) => {
      //    let boundaryX = { min: x - n < 0 ? 0 : x - n, max: x + n >= row.length ? row.length : x + n + 1 };
      //    row.slice(boundaryX.min, boundaryX.max).forEach((cell, x2) => {
      //       if (ManhattanDistance(y, x, y2, x2) <= n && !cell.isNeighbor) {
      //          cell.isNeighbor = true;
      //          neighborCount++;
      //       }
      //    });
      // });
      // matrix.slice(boundaryY.min, boundaryY.max + 1).forEach((r,u) => {
      //    let boundaryX = { min: x - n < 0 ? 0 : x - n, max: x + n > r.length ? r.length : x + n };
      //    console.log("BB",boundaryY)
      //    r.slice(boundaryX.min, boundaryX.max + 1).forEach((c, v)=>{
      //       console.log(v, boundaryX)
      //    })
      // });


      for (let nY = boundaryY.min; nY < boundaryY.max; nY++) {
         let boundaryX = { min: x - n < 0 ? 0 : x - n, max: x + n >= matrix[nY].length ? matrix[nY].length : x + n + 1 };
         for (let nX = boundaryX.min; nX < boundaryX.max; nX++) {
            let neighborCell = matrix[nY][nX];
            if (ManhattanDistance(y, x, nY, nX) <= n && !neighborCell.isNeighbor) {
               neighborCell.isNeighbor = true;
               neighborCount++;
            }
         }
      }

      return neighborCount;
   }
   
   matrix.forEach((row, y) => {
      row.forEach((cell, x) => {
         if (cell.positive) {
            if (!cell.isNeighbor) {
               cell.isNeighbor = true;
               neighborhood++;
            }
            positiveCells.push(cell);
            neighborhood += find(y, x);
         }
      });
   });
   
   console.log("# of cells in the Neighborhood:", neighborhood);

}

const printGrid = (matrix) => {
   let output = `${TLC}${HBP.repeat(matrix[0].length * 2 + 2)}${TRC}`

   matrix.forEach((row, y) => {
      let row_text = `\n${VBP}`; 
      row.forEach((cell, x) => {
         row_text += ` ${cell.positive ? HIT :cell.isNeighbor ? NEIGHBOR : MISS}`;
      })
      output += `${row_text}${" ".repeat(2)}${VBP}`;
   })
   output += `\n${BLC}${HBP.repeat(matrix[0].length * 2 + 2)}${BRC}`;
   return output;
   // for (let y = 0; y < matrix.length; y++) {
   //    let row = `\n${VBP}`; 
   //    for (let x = 0; x < matrix[0].length; x++) {
   //       row += ` ${matrix[y][x].positive ? HIT : matrix[y][x].isNeighbor ? NEIGHBOR : MISS}`;
   //    }
   //    // ret += `${row} ║\n${VBP}${" ".repeat(this.width * 2 + 1)}${VBP}`;
   //    output += `${row}${" ".repeat(2)}${VBP}`;
   // }
   // output += `\n${BLC}${HBP.repeat(matrix.length * 2 + 2)}${BRC}`;
   // return output;
};

const test1 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test2 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test3 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, 1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test4 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test5 = Object.freeze(makeCells([[1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test6 = Object.freeze(makeCells([[1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 1]]));

const test7 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1]]));

const test00 = Object.freeze(makeCells([[1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 1]]));


// console.log("CELL:", new Cell(-1, 0,0))
// console.log("TEST:", test4)
// let m = new Matrix({matrix:test5}); 
// m.analyzeGrid(m.grid);
// console.log(m.printGrid()); 

analyzeGrid(test1);
console.log(printGrid(test1))

analyzeGrid(test2);
console.log(printGrid(test2))

analyzeGrid(test3, 2);
console.log(printGrid(test3))

analyzeGrid(test4, 2);
console.log(printGrid(test4))


analyzeGrid(test5, 6);
console.log(printGrid(test5))

analyzeGrid(test00, 9);
console.log(printGrid(test00))

function cell(y, x) {return { y, x }};

// let mm = MatrixGenerator(5, 5, [
//    cell(1, 0),
//    cell(3, 4),
//    cell(4, 3)
// ]);

let mm = MatrixGenerator(13, 3, "{h:1,yy:1}");
// console.log(mm)
// console.log(printGrid(mm))


analyzeGrid(mm, 1);
console.log(printGrid(mm))

















