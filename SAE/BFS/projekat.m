clc;
clear;

load('matrica.txt');
[nbRows, nbColumns] = size(matrica);


G.AdjMatrix = generateTableAdjMatrix(nbRows, nbColumns);

nizP = [];
nizZ = [];

for i=1:nbRows
  for j=1:nbColumns
    if(matrica(i,j) == 1)
      nizP = [nizP (i*10+j-10)];
    else
      nizZ = [nizZ (i*10 + j - 10)];
    end
  end
end


G = defineNodesType(G, nizP, 'P'); %PUT
G = defineNodesType(G, nizZ, 'Z'); %ZID
Gp = BFS(G, 5);

path = getPath(Gp, 5, 94);
plotTable(G, path);