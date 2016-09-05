Shader "Vertex Colors" {
SubShader { 
 Pass {
  BindChannels {
   Bind "vertex", Vertex
   Bind "color", Color
   Bind "texcoord", TexCoord
  }
 }
}
}