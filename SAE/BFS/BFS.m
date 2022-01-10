function G = BFS(G, s)
  v = 1:length(G.AdjMatrix);
  for u = v(v~=s)
    G.V(u).color = 'W';
    G.V(u).d = Inf;
    G.V(u).pred = NaN;
  end

  G.V(s).color = 'W';
  G.V(s).d = 0;
  G.V(s).pred = NaN;
  
  %red Q
  Q = [];
  Q = [Q s];  %enqueue
  
  while ~isempty(Q)
    u = Q(1);
    Q(1) = []; %dequeue
    
    for v = find(G.AdjMatrix(u,:))
      if G.V(v).color == 'W' && G.V(v).type == 'P'
        G.V(v).color = 'G';
        G.V(v).d = G.V(u).d + 1;
        G.V(v).pred = u;
        Q = [Q v]; %[v Q] dodaje na pocetak
      end
    end
    G.V(u).color = 'B';
  end
end

