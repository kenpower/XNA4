using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CityShooter
{
    class BuildingFactory
    {
        public static String buildingSymbols = "123456789";
        static float storyHeight = 3;//meters
        static Texture2D[] textures;
        static Texture2D[] roofTextures;
        static Rectangle blockSize;

        public static Building makeBuilding(char c,Vector2 position)
        {
            Building b = new Building(theGame);
            int stories = c - '0';
            b.Texture = textures[stories];
            b.Height = stories * storyHeight;
            b.Position = new Vector3(position.X * blockSize.Width,0,position.Y*blockSize.Height);
            b.Size = blockSize;
            b.RoofTexture = roofTextures[stories % 4];
   
            b.Init();
            return b;
        }

        static Game theGame;

        public static void Init(Game game, Rectangle bS)
        {
            theGame = game;
            LoadBuildingTextures();
            blockSize=bS;
            storyHeight = blockSize.Width / 3;
        }

        static void LoadBuildingTextures()
        {
            textures = new Texture2D[10];

            ContentManager contentManger = (ContentManager)theGame.Services.GetService(typeof(ContentManager));

            textures[0] = contentManger.Load<Texture2D>("building1");
            textures[1] = contentManger.Load<Texture2D>("building1");
            textures[2] = contentManger.Load<Texture2D>("building2");
            textures[3] = contentManger.Load<Texture2D>("building3");
            textures[4] = contentManger.Load<Texture2D>("building3");
            textures[5] = contentManger.Load<Texture2D>("building6");
            textures[6] = contentManger.Load<Texture2D>("building6");
            textures[7] = contentManger.Load<Texture2D>("building7");
            textures[8] = contentManger.Load<Texture2D>("building8");
            textures[9] = contentManger.Load<Texture2D>("building8");

            roofTextures = new Texture2D[4];
            roofTextures[0] = contentManger.Load<Texture2D>("roof1");
            roofTextures[1] = contentManger.Load<Texture2D>("roof2");
            roofTextures[2] = contentManger.Load<Texture2D>("roof3");
            roofTextures[3] = contentManger.Load<Texture2D>("roof4");


        }

    }
    public class Building: GameObject
    {
        float height;

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        Texture2D texture;
        Texture2D roofTexture;

        public Texture2D RoofTexture
        {
            get { return roofTexture; }
            set { roofTexture = value; }
        }


        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }


        Rectangle size;

        public Rectangle Size
        {
            get { return size; }
            set { size = value; }
        }

        BoundingBox boundingBox;

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }


        public Building(Game game):base(game)
        {
        

        }

        VertexPositionNormalTexture[] vertices;
        short[] indices;
        int numWallTriangles;
        int numRoofTriangles;
        int numWallVertices;
        int numRoofVertices;
        int numVertices;
        int offsetRoofIndices;


        public override void Init()
        {
            Effect = new BasicEffect(graphicsDevice);
            Effect.VertexColorEnabled = false;
            Effect.TextureEnabled = true;


            numWallVertices = 8;
            numRoofVertices = 4;
            numVertices = numWallVertices + numRoofVertices;
            vertices = new VertexPositionNormalTexture[numVertices];

            
            vertices[0].Position = new Vector3(Position.X,0,Position.Z);
            vertices[0].TextureCoordinate = new Vector2(0,1);

            vertices[1].Position = new Vector3(Position.X+size.Width, 0, Position.Z);
            vertices[1].TextureCoordinate = new Vector2(1,1);

            vertices[2].Position = new Vector3(Position.X+size.Width, 0, Position.Z+size.Height);
            vertices[2].TextureCoordinate = new Vector2(0,1);

            vertices[3].Position = new Vector3(Position.X, 0, Position.Z + size.Height);
            vertices[3].TextureCoordinate = new Vector2(1,1);

            vertices[4].Position = new Vector3(Position.X, height, Position.Z);
            vertices[4].TextureCoordinate = new Vector2(0,0);

            vertices[5].Position = new Vector3(Position.X + size.Width, height, Position.Z);
            vertices[5].TextureCoordinate = new Vector2(1,0);

            vertices[6].Position = new Vector3(Position.X + size.Width, height, Position.Z + size.Height);
            vertices[6].TextureCoordinate = new Vector2(0,0);

            vertices[7].Position = new Vector3(Position.X, height, Position.Z + size.Height);
            vertices[7].TextureCoordinate = new Vector2(1,0);

            //roof vertices below
           
            vertices[8].Position = new Vector3(Position.X,              height,     Position.Z+ size.Height);
            vertices[8].TextureCoordinate = new Vector2(0, 1);

            vertices[9].Position = new Vector3(Position.X,              height,     Position.Z );
            vertices[9].TextureCoordinate = new Vector2(0, 0);

            vertices[10].Position = new Vector3(Position.X + size.Width, height,    Position.Z + size.Height);
            vertices[10].TextureCoordinate = new Vector2(1, 1);

            vertices[11].Position = new Vector3(Position.X + size.Width, height,    Position.Z );
            vertices[11].TextureCoordinate = new Vector2(1, 0);

            numWallTriangles = 8;
            numRoofTriangles = 2;
            indices = new short[numWallTriangles+ 2 + numRoofTriangles + 2];

            int i=0;
            indices[i++] = 0;
            indices[i++] = 4;
            indices[i++] = 1;
            indices[i++] = 5;
            indices[i++] = 2;
            indices[i++] = 6;
            indices[i++] = 3;
            indices[i++] = 7;
            indices[i++] = 0;
            indices[i++] = 4;

            //roof indices
            offsetRoofIndices = i;           
            indices[i++] = 8;
            indices[i++] = 9;
            indices[i++] = 10;
            indices[i++] = 11;



            boundingBox = new BoundingBox(vertices[0].Position, vertices[6].Position);



        }

        public void Update(GameTime gametime)
        {
        }

        public void Draw(GameTime gametime, Camera camera)
        {
            Effect.View = camera.View;

            Effect.Projection = camera.Projection;
            Effect.World = Matrix.Identity;
            Effect.Texture = texture;

            graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, vertices, 0, numVertices, indices, 0, numWallTriangles);

            }

            Effect.Texture = roofTexture;


            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, vertices, 0, numVertices, indices, offsetRoofIndices, numRoofTriangles);

            }




   
        }


    }
}
