function path = getPath(G, idStart, idEnd)
    path = []; 
    temp = idEnd;

    while (temp~=idStart)
        path = [temp path];
        temp = G.V(temp).pred;
    end

    path = [idStart path];
end 