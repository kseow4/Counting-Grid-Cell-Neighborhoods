

const H = 11;
const W = 11;
const N = 3;
const HIT = `▩`;
const NEIGHBOR = `☒`;
const MISS = `☐`;

 /**
  * Cell Class to contain a signed numeric value and the neighbor flag.
  */
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
 * @param {*} y1 
 * @param {*} x1 
 * @param {*} y2 
 * @param {*} x2 
 * @returns {number} The sum of the differences in the two dimensions (Y, X) from the given Cells.
 */
const ManhattanDistance = (y1, x1, y2, x2) => Math.abs(y1 - y2) + Math.abs(x1 - x2);

/**
 * 
 * @param {*} y1 
 * @param {*} x1 
 * @param {*} y2 
 * @param {*} x2 
 * @param {*} h 
 * @param {*} w 
 * @returns 
 */
const ManhattanDistanceWrapped = (y1, x1, y2, x2, h, w) => 
   Math.min(Math.abs(y1 - y2), h - Math.abs(y1 - y2)) +
   Math.min(Math.abs(x1 - x2), w - Math.abs(x1 - x2));

/**
 * 
 * @param {*} y1 
 * @param {*} x1 
 * @param {*} y2 
 * @param {*} x2 
 * @param {*} h 
 * @param {*} w 
 * @returns 
 */
const EuclideanDistanceWrapped = (y1, x1, y2, x2, h, w) => Math.sqrt(
   Math.pow(Math.min(Math.abs(y1 - y2), h - Math.abs(y1 - y2)), 2) +
   Math.pow(Math.min(Math.abs(x1 - x2), w - Math.abs(x1 - x2)), 2));
 
/**
 * 
 * @param {*} y1 
 * @param {*} x1 
 * @param {*} y2 
 * @param {*} x2 
 * @param {*} h 
 * @param {*} w 
 * @returns 
 */
const ChebyshevDistanceWrapped = (y1, x1, y2, x2, h, w) => Math.max(
   Math.min(Math.abs(y1 - y2), h - Math.abs(y1 - y2)),
   Math.min(Math.abs(x1 - x2), w - Math.abs(x1 - x2)));

/**
 * Creates a 2D Array of Cell objects given a 2D Array of numbers.
 * 
 * @param {number[][]} matrix A 2D Array of numeric values. 
 * @returns {Cell[][]} A 2D Array of Cell objects.
 */
function makeCells(matrix) {
   try {
      let moc = [];
      let same_length;
      matrix.forEach(row => {
         if (!same_length) { same_length = row.length; }
         if (same_length != row.length) throw `Invalid Matrix!\nRow [${matrix.indexOf(row)}] is inconsistent in length.`;
         let roc = [];
         row.forEach(cell => {
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
 * @param {*} index 
 * @param {*} size 
 * @returns 
 */
const wrap = (index, size) => (index + size) % size;
 
/**
 * Generates a neighborhood with the given height and width - and allows for specification on how many positive cells
 * 
 * @param {*} height 
 * @param {*} width 
 * @param {*} positives 
 * @returns a 2D array 
 */
function NeighborhoodGenerator(height, width, positives) { 
   try {
      let neighborhood = Array.from({ length: height }, () => (Array.from({ length: width }, () => positives ? new Cell() : new Cell(RandomValueGenerator()))));

      switch(typeof positives) {
         case 'object':
            if (Array.isArray(positives)) { 
               positives.forEach(cell => { 
                  if (cell.y > height || cell.y < 0 || !Number.isInteger(cell.y)) throw `Cell (${cell.y}, ${cell.x}) has an invalid Y value: ${cell.y}.`;
                  if (cell.x > width || cell.x < 0 || !Number.isInteger(cell.x)) throw `Cell (${cell.y}, ${cell.x}) has an invalid X value: ${cell.x}.`;
                  neighborhood[cell.y][cell.x].value = Math.abs(neighborhood[cell.y][cell.x].value);
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
               neighborhood[y][x].value = Math.abs(neighborhood[y][x].value);
            }
            break;

         default:
            throw `Invalid input type [${typeof positives}]: ${positives}.`;

      }

      return neighborhood; 
   } catch (e) { console.error(e); }
};

/**
 * Finds the number of neighbors of a given neighborhood
 * 
 * @param {*} neighborhood 
 * @param {*} n 
 * @returns Neighbhorhood count
 */
function FindNeighbors(neighborhood, {n = N, distanceType = "m", print = true}) {
   // let positiveCells = []; 
   const height = neighborhood.length;
   const width = neighborhood[0].length;
   let neighborhoodCount = 0; 

   let distanceFunc;
   switch (distanceType.toLowerCase()) {
      case "m":
      case "manhattan":
         distanceFunc = ManhattanDistanceWrapped;
         break;
      case "e":
      case "euclidean":
         distanceFunc = EuclideanDistanceWrapped;
         break;
      case "c":
      case "chebyshev":
         distanceFunc = ChebyshevDistanceWrapped;
         break;
      default:
         distanceFunc = ManhattanDistanceWrapped;
         break;
   }

   /**
    * 
    * 
    * @param {number} y 
    * @param {number} x 
    * @returns {number} The number of neighboring cells.
    */
   // const find = (y, x) => {
   //    let neighborCount = 0;
   //    let boundaryY = { min: y - n < 0 ? 0 : y - n, max: y + n >= neighborhood.length ? neighborhood.length : y + n + 1};

   //    for (let nY = boundaryY.min; nY < boundaryY.max; nY++) {
   //       console.log("nY", nY)

   //       let boundaryX = { min: x - n < 0 ? 0 : x - n, max: x + n >= neighborhood[nY].length ? neighborhood[nY].length : x + n + 1 };
   //       let xmax = (x + n) - neighborhood[nY].length;
   //       // console.log("xmax", xmax)
         
   //       let xmin = (x - n) + neighborhood[nY].length; 
   //       // console.log("xmin", xmin)
         

   //       for (let nX = boundaryX.min; nX < boundaryX.max; nX++) {
   //          let neighborCell = neighborhood[nY][nX];
   //          if (ManhattanDistance(y, x, nY, nX) <= n && !neighborCell.isNeighbor) {
   //             neighborCell.isNeighbor = true;
   //             neighborCount++;
   //          }
   //       }
   //       // for (let nX = 0; nX < xmax; nX++) {
   //       //    let neighborCell = neighborhood[nY][nX];
   //       //    if (ManhattanDistance(y, x, nY, nX) <= n && !neighborCell.isNeighbor) {
   //       //       neighborCell.isNeighbor = true;
   //       //       neighborCount++;
   //       //    }
   //       // }
   //       for (let nX = xmin; nX < neighborhood[nY].length ; nX++) {
   //          // let neighborCell = neighborhood[nY][nX];
   //          let wrappedY = wrap(nY, neighborhood.length);
   //          let wrappedX = wrap(nX, neighborhood[0].length);
   //          let neighborCell = neighborhood[wrappedY][wrappedX];

   //          console.log("xmin", xmin)
   //          console.log("row", nY)
   //          console.log("manhattan", ManhattanDistance(y, x, nY, nX))


   //          console.log("MD", ManhattanDistance(y, x, nY, nX) + n)
   //          if (ManhattanDistance(y, x, nY, nX) && !neighborCell.isNeighbor) {
   //             neighborCell.isNeighbor = true;
   //             neighborCount++;
   //          }
   //       }

   //    }



   //    return neighborCount;
   // }
   const find = (y, x) => {
      let neighborCount = 0;

      for (let dy = -n; dy <= n; dy++) {
         for (let dx = -n; dx <= n; dx++) {
            const wrappedY = wrap(y + dy, height);
            const wrappedX = wrap(x + dx, width);
            const dist = distanceFunc(y, x, wrappedY, wrappedX, height, width);

            if (dist <= n) {
               let neighborCell = neighborhood[wrappedY][wrappedX];
               if (!neighborCell.isNeighbor) {
                  neighborCell.isNeighbor = true;
                  neighborCount++;
               }
            }
         }
      }
      return neighborCount;
   }
   
   neighborhood.forEach((row, y) => {
      row.forEach((cell, x) => {
         if (cell.positive) {
            if (!cell.isNeighbor) {
               cell.isNeighbor = true;
               neighborhoodCount++;
            }
            // positiveCells.push(cell);
            neighborhoodCount += find(y, x);
         }
      });
   });
   
   console.log(`${distanceFunc.name} # of cells in the Neighborhood:`, neighborhoodCount);
   if (print) {
      console.log(PrintNeighborhood(neighborhood));
   }
   return neighborhoodCount;
}

/**
 * Prints the neighborhood
 * 
 * @param {*} neighborhood 
 * @returns 
 */
function PrintNeighborhood(neighborhood) { 
   let output = "";

   neighborhood.forEach(row => { 
      let row_text = "";
      row.forEach(cell => {
         row_text += `${cell.positive ? HIT :cell.isNeighbor ? NEIGHBOR : MISS} `;
      }); 
      output += `${row_text}\n`;
   }); 
   return output;
};


// Tests
const test1 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test2 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test3 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, 1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test4 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test5 = Object.freeze(makeCells([[1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1]]));

const test6a = Object.freeze(makeCells([[1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 1]]));
const test6b = Object.freeze(makeCells([[1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 1]]));
const test6c = Object.freeze(makeCells([[1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 1]]));

const test7 = Object.freeze(makeCells([[-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1]]));

const test8 = Object.freeze(makeCells([[1, -1, -1, -1, -1, 1, -1, -1, -1, -1, 1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [1, -1, -1, -1, -1, 1, -1, -1, -1, -1, 1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1], [1, -1, -1, -1, -1, 1, -1, -1, -1, -1, 1]]));

// FindNeighbors(test1);
// console.log(PrintNeighborhood(test1))

// FindNeighbors(test2);
// console.log(PrintNeighborhood(test2))

// FindNeighbors(test3, 2);
// console.log(PrintNeighborhood(test3))

// FindNeighbors(test4, 2);
// console.log(PrintNeighborhood(test4))

// FindNeighbors(test5, 6);
// console.log(PrintNeighborhood(test5))

FindNeighbors(test6a, {distanceType: "e"});
FindNeighbors(test6b, {distanceType: "c"});
FindNeighbors(test6c, {distanceType: "m"});


// console.log(PrintNeighborhood(test6))

// FindNeighbors(test7);
// console.log(PrintNeighborhood(test7))

// FindNeighbors(test8, 2);
// console.log(PrintNeighborhood(test8))

// function cell(y, x) {return { y, x }};

// let test9 = NeighborhoodGenerator(5, 5, [
//    cell(1, 0),
//    cell(3, 4),
//    cell(4, 3)
// ]);

// FindNeighbors(test9, 1);
// console.log(PrintNeighborhood(test9))
 












