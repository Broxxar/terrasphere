# Terrashere
_A Zen Planet Painter_

Terrasphere is an atmospheric toy/tech demo. 3D Paint the land masses onto your tiny world and chill out to a Trance beat while admiring the beauty of your creation and the infinite space beyond!

Terraspshere was made in 48 hours for [Ludum Dare](https://ldjam.com)!

### Controls

Left Click: Use Active Tool
Number Keys: Select Tool (or click the hot bar)
Middle Mouse/Right Click: Rotate Camera
Mouse Scroll Wheel: Zoom In/Out

### Links
* [Download from itch.io!](https://broxxar.itch.io/terrasphere)
* [Twitter](https://twitter.com/DanielJMoran)

### Notes
The code is pretty much throwaway garabge. That said, there are a few interesting things worth looking at (despite the fact that they are buried in the heaping pile of garbage).

I wanted to have completely seamless heightmaps projected onto a sphere. I did this setup involving multiple RenderTextures and capturing them into a Cubemap. This gave me height to offset verts (with tex2Dlod) and a value I could feed into a LUT to color the terrain. And then I did something heinous/interesting...

To get the appearance of a normal map out of the sphere-cubemap-heightmap, I took several samples from the cube to deterimine the rate of change across the height, and just sort of... added this to the normalized object space point to get what looks pretty much like an object space normal. By transforming the light and view directions into object space, I could do a rough but passable light and fernsel on my planets surface without having precalculated normals.

All that said, if I were to do it again, I would move a bunch of this crap off the GPU and just have a mesh I can mold on the CPU, and find some other way of getting textures/normals onto the surface. At least I might try that approach. This ended up being horribly complicated and definitely incorrect.

But hey, it doesn't look half bad.
