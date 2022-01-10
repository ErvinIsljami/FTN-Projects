function [ G ] = defineNodesType( G, nodes, type )
    for u = nodes
        G.V(u).type = type;       
    end
end

