function [ adjMatrix ] = generateTableAdjMatrix(nbRows, nbColumns)
    adjMatrix = zeros(nbRows*nbColumns, nbRows*nbColumns);
    adjMatrixRowCounter = 0;
    
    for i = 1:nbRows
       for j = 1:nbColumns
           currentId = (i-1)*nbColumns + j;
           neighbourRow = [currentId-1 currentId currentId+1];
           neighbourMatrix = [neighbourRow-nbColumns; neighbourRow; neighbourRow + nbColumns];

           startRowIndex = 1;
           startColumnIndex = 1;
           endRowIndex = 3;
           endColumnIndex = 3;

           if i == 1
               startRowIndex = 2;
           end
           if j == 1
               startColumnIndex = 2;
           end
           if i == nbRows
              endRowIndex = 2;
           end
           if j == nbColumns
              endColumnIndex = 2; 
           end

           neighbours = [];
           for m = startRowIndex:endRowIndex
              for n = startColumnIndex:endColumnIndex
                  if neighbourMatrix(m,n)~=currentId
                    neighbours = [neighbours neighbourMatrix(m,n)];
                  end
              end
           end

           adjRow = zeros(1, length(adjMatrix));
           for m = 1:length(neighbours)
                adjRow(neighbours(m)) = 1;
           end
           
           adjMatrixRowCounter  = adjMatrixRowCounter + 1;           
           adjMatrix(adjMatrixRowCounter, :) = adjRow;          
       end
    end
end

