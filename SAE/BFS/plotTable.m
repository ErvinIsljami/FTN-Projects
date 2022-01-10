function [] = plotTable(G, path)

if ~isfield(G, 'AdjMatrix')
    disp('Ne postoji matrica susedstva u grafu. Graf se ne moze prikazati.');
    return
end
if length(G.AdjMatrix) < 1
    disp('Matrica susedstva je prazna. Graf se ne moze prikazati.');
    return 
end    

colors = ['#FFFFFF' '#C0C0C0' '#808080' '#008000' '#000080' '#008080' '#800000' '#800080' '#808000' '#0000FF' '#00FF00' '#00FFFF' '#FF0000' '#FF00FF' '#FFFF00' '#000000'];
searchedNodeTypes = ['W' 'G' 'B'];
searchedNodeTypeColorIDs = [1 2 3];

nodeTypes = [];
nodeTypeColorIDs = [];

if isfield(G, 'nodeTypeColorIDs')
    nodeTypeColorIDs = G.nodeTypeColorIDs;   
end

if isfield(G, 'nodeTypes')
    nodeTypes = G.nodeTypes; 
else
    if isfield(G, 'V') && isfield(G.V(1), 'type')
            for i = 1:length(G.AdjMatrix)                
                tempType = G.V(i).type;               
                if (~any(nodeTypes == tempType))                   
                    nodeTypes = [nodeTypes tempType];                    
                    if ~isfield(G, 'nodeTypeColorIDs')                        
                        nodeTypeColorIDs = [ nodeTypeColorIDs length(nodeTypes)];                        
                    end
                end                
            end        
    end
end

content = '';

if nargin==2 && ~isempty(path); 
    tempPath = zeros(length(path)-1,2);
    for i=1:length(path)-1;
        tempPath(i,:) = [path(i) path(i+1)];
    end  
end

%  DOT EDGES
for i = 1:length(G.AdjMatrix)
    adjRow = G.AdjMatrix(i,:);
    edges = find(adjRow ~= 0);    
    for j = 1:length(edges)        
        content = [content num2str(i) '->' num2str(edges(j)) ''];         
        if isfield(G, 'Weights')
            weightsRow = G.Weights(i,:);
            weights = weightsRow(adjRow ~= 0);
            content = [content '[ label = "' num2str(weights(j)) '" weight="1"]'];
        end          
        if nargin==2 && ~isempty(path);            
            connection = [i edges(j)];
			[m, n] = size(tempPath);            
            for k = 1:m
                if(tempPath(k,:) == connection)
                    %content = [content '[color=red penwidth=3.0]'];
                    break
                end
            end             
        end        
        content = [content ';'];        
    end
end

%   DOT NODES
for i = 1:length(G.AdjMatrix)
    nodeContent = '';
    colorIndex = 1;
    nodeColor = ['fillcolor = "' colors((colorIndex-1)*7+1 : colorIndex*7) '"'];   
    if isfield(G, 'V')        
        fieldNames = fieldnames(G.V);       
        for j = 1:length(fieldNames)
            fieldValue = G.V(i).(fieldNames{j});            
            if isnumeric(fieldValue)
               fieldValue = num2str(fieldValue);
            end            
            nodeContent = [nodeContent fieldNames{j} ': ' fieldValue '\n'];  
        end        
        if isfield(G.V(i), 'type')
            colorIndex = nodeTypeColorIDs(nodeTypes == G.V(i).type);              
        end      
        if isfield(G.V(i), 'color')            
            colorIndex = searchedNodeTypeColorIDs(searchedNodeTypes == G.V(i).color);           
        end        
        nodeColor = ['fillcolor = "' colors((colorIndex-1)*7+1 : colorIndex*7) '"'];                   
    end
    if nargin==2 && any(path == i)
        step = find(path == i);
        colorIndex = 13;
        nodeColor = ['fillcolor = "' colors((colorIndex-1)*7+1 : colorIndex*7) '"'];
        nodeContent = [nodeContent '[' num2str(step) ']\n'];  
    end
    content = [content num2str(i) ' [' nodeColor, 'style=filled, label="' num2str(i) '\n' nodeContent '"]'];           
    content = [content ';'];
end

ranks = '';
%ranks = '{rank=same 1 2 3}{rank=same 4 5 6}{rank=same 7 8 9}';
graphStrDot = 'digraph { graph [ pad=".2", ranksep=".2", nodesep=".2" ] overlap = scale compound = true splines = line node[shape = record, style = bold]';
bracket = '}';
graphStrDot = [graphStrDot content ranks bracket];

[nrows, ncols] = size(graphStrDot);
fid = fopen('output.dot', 'w');
for k = 1:nrows, fprintf(fid, '%s\n', graphStrDot(k,:)); end;
fclose(fid);

%OCTAVE
system('neato -Tjpeg output.dot > output.jpeg');
imshow(imread('output.jpeg'));

%MATLAB
%!C:\Graphviz2.38\bin\neato.exe -Tjpeg output.dot -o output.jpeg
%imshow(imread('output.jpeg'))
