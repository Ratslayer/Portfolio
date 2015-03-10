using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RedStream
{
    public class RsGyroscope : RsActor
    {
        public enum GyroMode
        {
            OuterMaster,
            InnerMaster
        };
        public new class Desc : RsActor.Desc
        {
            public Desc()
                : base()
            {
            }
        }
        public int MaxTowersPerGyro = 16;
        public RsGyroscope(Desc desc)
            : base(desc)
        {
            desc.MaterialDesc.Color = new Vector4(1, .4f, .7f, 1);
            OuterRing = new RsGyro(desc, this);
            desc.MaterialDesc.Color = new Vector4(.4f, 1, .7f, 1);
            desc.Scale *= 0.8f;
            MiddleRing = new RsGyro(desc, this);
            desc.MaterialDesc.Color = new Vector4(.4f, .7f, 1, 1);
            desc.Scale *= 0.8f;
            InnerRing = new RsGyro(desc, this);
            SelectedRing = InnerRing;
            Visible = false;
            Mode = GyroMode.OuterMaster;
            GyroRotations = Vector3.Zero;
        }
        public RsActor SelectedRing { get; private set; }
        private RsActor OuterRing, MiddleRing, InnerRing;
        private Vector3 GyroRotations;
        private GyroMode Mode;
        public void SelectNextRing()
        {
            if (SelectedRing == InnerRing)
                SelectedRing = MiddleRing;
            else SelectedRing = OuterRing;
        }
        public void SelectPreviousRing()
        {
            if (SelectedRing == OuterRing)
                SelectedRing = MiddleRing;
            else SelectedRing = InnerRing;
        }
        public void RotateRing(float amount)
        {
            RotateRing(SelectedRing, amount);
        }
        public string GetSelectedRingName()
        {
            return GetRingName(SelectedRing);
        }
        public string GetRingName(RsActor ring)
        {
            if (ring == OuterRing)
                return "Outer";
            else if (ring == MiddleRing)
                return "Middle";
            else if(ring == InnerRing)
                return "Inner";
            else return "I AM ERROR!";
        }
        public string GetModeName()
        {
            if (Mode == GyroMode.InnerMaster)
                return "Inner";
            else if (Mode == GyroMode.OuterMaster)
                return "Outer";
            else return "I AM ERROR!";
        }
        public void ToggleMode()
        {
            if (Mode == GyroMode.InnerMaster)
                Mode = GyroMode.OuterMaster;
            else if (Mode == GyroMode.OuterMaster)
                Mode = GyroMode.InnerMaster;
            /*float tempValue = GyroRotations.X;
            GyroRotations.X = GyroRotations.Z;
            GyroRotations.Z = tempValue;*/
        }
        private void RotateRing(RsActor ring, float amount)
        {
            //Quaternion quat;
            if (ring == OuterRing)
            {
                //quat = Quaternion.CreateFromYawPitchRoll(0, MathHelper.ToRadians(amount), 0);
                //Rotator.Yaw += amount;
                GyroRotations.X += amount;
            }
            else if (ring == MiddleRing)
            {
                //quat = Quaternion.CreateFromYawPitchRoll(0, 0, MathHelper.ToRadians(amount));
                //Rotator.Pitch += amount;
                GyroRotations.Y += amount;
            }
            else if (ring == InnerRing)
            {
                //quat = Quaternion.CreateFromYawPitchRoll(0, MathHelper.ToRadians(amount), 0);
                //Rotator.Roll += amount;
                GyroRotations.Z += amount;
            }
            else return;
            
        }
        /*private void RotateRing(RsActor ring, Quaternion amount)
        {
            if (ring == OuterRing)
            {
                RotateRing(MiddleRing, amount);
            }
            else if (ring == MiddleRing)
            {
                RotateRing(InnerRing, amount);
            }
            ring.Orientation *= amount;
        }
        private void ApplyRotator()
        {
            RsRotator tempRot = new RsRotator(Rotator);
            InnerRing.Orientation = tempRot.ToQuat();
            tempRot.Roll = 0;
            MiddleRing.Orientation = tempRot.ToQuat();
            tempRot.Pitch = 0;
            OuterRing.Orientation = tempRot.ToQuat();
        }*/
        private void ApplyRotations()
        {
            Quaternion OuterQuat, MiddleQuat, InnerQuat;
            OuterQuat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), MathHelper.ToRadians(GyroRotations.X));
            MiddleQuat = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), MathHelper.ToRadians(GyroRotations.Y));
            InnerQuat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), MathHelper.ToRadians(GyroRotations.Z));
            if (Mode == GyroMode.OuterMaster)
            {
                OuterRing.Orientation = OuterQuat;
                MiddleRing.Orientation = OuterQuat * MiddleQuat;
                InnerRing.Orientation = OuterQuat * MiddleQuat * InnerQuat;
            }
            else if (Mode == GyroMode.InnerMaster)
            {
                InnerRing.Orientation = InnerQuat;
                MiddleRing.Orientation = InnerQuat * MiddleQuat;
                OuterRing.Orientation = InnerQuat * MiddleQuat * OuterQuat;
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //RotateRing(ring, quat);
            //ApplyRotator();
            ApplyRotations();
        }
    }
}
