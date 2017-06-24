using Resources.Utilities;
using System.IO;

namespace Resources.Packet.Part
{
    public class Appearance
    {
        public byte unknownA;
        public byte unknownB;
        public byte hair_red;
        public byte hair_green;
        public byte hair_blue;
        public short flags;
        public byte unknownC; //padding?
        public FloatVector character_size = new FloatVector();
        public short head_model;
        public short hair_model;
        public short hand_model;
        public short foot_model;
        public short body_model;
        public short tail_model;
        public short shoulder2_model;
        public short wings_model;
        public float head_size;
        public float body_size;
        public float hand_size;
        public float foot_size;
        public float shoulder2_size;
        public float weapon_size;
        public float tail_size;
        public float shoulder_size;
        public float wings_size;
        public float body_rotation;
        public FloatVector hand_rotation = new FloatVector();
        public float feet_rotation;
        public float wing_rotation;
        public float tail_rotation;
        public FloatVector body_offset = new FloatVector();
        public FloatVector head_offset = new FloatVector();
        public FloatVector hand_offset = new FloatVector();
        public FloatVector foot_offset = new FloatVector();
        public FloatVector back_offset = new FloatVector();
        public FloatVector wing_offset = new FloatVector();

        public void read(BinaryReader reader)
        {
            unknownA = reader.ReadByte();
            unknownB = reader.ReadByte();
            hair_red = reader.ReadByte();
            hair_green = reader.ReadByte();
            hair_blue = reader.ReadByte();
            flags = reader.ReadInt16();
            unknownC = reader.ReadByte();
            character_size.read(reader);
            head_model = reader.ReadInt16();
            hair_model = reader.ReadInt16();
            hand_model = reader.ReadInt16();
            foot_model = reader.ReadInt16();
            body_model = reader.ReadInt16();
            tail_model = reader.ReadInt16();
            shoulder2_model = reader.ReadInt16();
            wings_model = reader.ReadInt16();
            head_size = reader.ReadSingle();
            body_size = reader.ReadSingle();
            hand_size = reader.ReadSingle();
            foot_size = reader.ReadSingle();
            shoulder2_size = reader.ReadSingle();
            weapon_size = reader.ReadSingle();
            tail_size = reader.ReadSingle();
            shoulder_size = reader.ReadSingle();
            wings_size = reader.ReadSingle();
            body_rotation = reader.ReadSingle();
            hand_rotation.read(reader);
            feet_rotation = reader.ReadSingle();
            wing_rotation = reader.ReadSingle();
            tail_rotation = reader.ReadSingle();
            body_offset.read(reader);
            head_offset.read(reader);
            hand_offset.read(reader);
            foot_offset.read(reader);
            back_offset.read(reader);
            wing_offset.read(reader);
        }

        public void write(BinaryWriter writer)
        {
            writer.Write(unknownA);
            writer.Write(unknownB);
            writer.Write(hair_red);
            writer.Write(hair_green);
            writer.Write(hair_blue);
            writer.Write(flags);
            writer.Write(unknownC);
            character_size.write(writer);
            writer.Write(head_model);
            writer.Write(hair_model);
            writer.Write(hand_model);
            writer.Write(foot_model);
            writer.Write(body_model);
            writer.Write(tail_model);
            writer.Write(shoulder2_model);
            writer.Write(wings_model);
            writer.Write(head_size);
            writer.Write(body_size);
            writer.Write(hand_size);
            writer.Write(foot_size);
            writer.Write(shoulder2_size);
            writer.Write(weapon_size);
            writer.Write(tail_size);
            writer.Write(shoulder_size);
            writer.Write(wings_size);
            writer.Write(body_rotation);
            hand_rotation.write(writer);
            writer.Write(feet_rotation);
            writer.Write(wing_rotation);
            writer.Write(tail_rotation);
            body_offset.write(writer);
            head_offset.write(writer);
            hand_offset.write(writer);
            foot_offset.write(writer);
            back_offset.write(writer);
            wing_offset.write(writer);
        }
    }
}
