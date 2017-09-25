using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxModel
{
    public enum PdrActionType
    {
        InhibitionHemicorps = 0,
        InhibitionHemichamp = 1,
        FacilitationAction = 2,
        PasDeModificationAction = 3,
        EgaliteLaterale = 4,
        EgaliteLateraleForce = 5,
        EgaliteLateraleTemps = 6,
        BiaisLateral = 7,
        BiaisLateralForce = 8,
        BiaisLateralTemps = 9,
        FacilitationLaterale = 10
    }

    public enum PdrOptions
    {
        Option1 = 0,
        Option2 = 1,
        PasOptions = 2
    }

    public enum PdrZoom
    {
        Petit = 0,
        Grand = 1
    }

    // TODO : tout changer pour les enums
    /// <summary>
    /// Compatible firmware versions. Only major number required.
    /// </summary>
    /// 

    public enum CompatibleFirmwareVersions
    {
        v1_x = 1
    }

    public enum FrameStart
    {
        Sart1 = 0xCF,
        Start2 = 0xDF
    }

    public enum FrameStop
    {
        Stop1 = 0xEF,
        Stop2 = 0xFF
    }

    /// <summary>
    /// Frame headers.
    /// </summary>
    /// <remarks>
    /// A matching enumeration exists in firmware source.
    /// </remarks>
    public enum FrameHeaders
    {
        Error = 0xF0,               // 000
        ACK_Stop = 0x01,            // 001    
        ACK_mod_suiv_traj = 0x03,   // 002
        ACK_mode_libre = 0x05,      // 005
        ACK_mod_traj = 0x07,        // 007
        ACK_mod_homing = 0x09,      // 009
        ACK_mod_init_traj = 0x0B,   // 011
        ACK_mod_home = 0x0D,        // 013
        ACK_mod_game = 0x0F,        // 015
        ACK_game_position = 0x15,   // 021
        ACK_Cibles = 0x5B,          // 091
        ACK_Mouvements = 0x5D,      // 093
        ACK_Formes = 0x5F,          // 095
        ACK_MasseVisco = 0x65,      // 101       
        ACK_KlatClong = 0x67,       // 103
        ACK_VitesseNbrsrep = 0x69,  // 105
        ACK_ActifPassif = 0x6B,     // 107
        ACK_mod_hemi_pdr = 0x49,
        Force = 0xB4,               // 180
        Force2 = 0xB6,              // 182
        Vitesse = 0xBC,             // 188
        Vitesse2 = 0xBE,            // 190
        Position = 0xC2,            // 194
        Position2 = 0xC4,           // 196
        ForceRap = 0xC6,            // 198
        ForceRap2 = 0xC8,           // 200
        Ppr = 0xCA,                 // 202
        ForceRef = 0xE2,            // 226
        ACK_StopNv = 0xF1,          // 241
        ACK_Xdent = 0x77,           // 119
        ACK_Acq = 0x79,             // 121
        ACK_End_Acq = 0x7B,         // 123
        ACK_Borne = 0X8D,           // 141
        BorneCirc = 0x8F,           // 143
        ACK_StreamingInit = 0xA1,   // 161
        ACK_StreamingSeg = 0xA3,    // 163
        ACK_StreamingPoint = 0xA5,  // 165
        ACK_StreamingError = 0xA7,  // 167
        ACK_StreamingMod = 0xA9,    // 169
        ACK_StreamingNext = 0xAB,   // 171
        ACK_modMazeCalib = 0xAD,    // 173
        ACK_modMazeGame = 0xAF,     // 175
        Hardware = 0x00,
        Software = 0x222222, // 22
        ACK_mod_pos = 0x23
    }

    /// <summary>
    /// Error codes.
    /// </summary>
    /// <remarks>
    /// A matching enumeration exists in firmware source.
    /// </remarks> 
    /// <summary>
    /// Error codes.
    /// </summary>
    /// <remarks>
    /// A matching enumeration exists in firmware source.
    /// </remarks> 
    public enum ErrorEmcyCodes
    {
        NoError = 0x000,
        GenericError = 0x1000,
        OvercurrentError = 0x2310,
        OvervoltageError = 0x3210,
        UndervoltageError = 0x3220,
        OvertemperatureError = 0x4210,
        LogicSupplyVoltageTooLowError = 0x5113,
        SupplyVoltageOutputStageTooLow = 0x5114,
        InternalSoftwareError = 0x6100,
        SoftwareParameterError = 0x6320,
        PositionSensorError = 0x7320,
        CanOverrunErrorObjectsLost = 0x8110,
        CanOverrunError = 0x8111,
        CanPassiveModeError = 0x8120,
        CanLifeGuardError = 0x8130,
        CanTransmitCobIdCollisionError = 0x8150,
        CanBusOffError = 0x81FD,
        CanRxQueueOverflowError = 0x81FE,
        CanTxQueueOverflowError = 0x81FF,
        CanPdoLengthError = 0x8210,
        FollowingError = 0x8611,
        HallSensorError = 0xFF01,
        IndexProcessingError = 0xFF02,
        EncoderResolutionError = 0xFF03,
        HallSensorNotFoundError = 0xFF04,
        NegativeLimitSwitchError = 0xFF06,
        PositiveLimitSwitchError = 0xFF07,
        HallAngleDetectionError = 0xFF08,
        SoftwarePositionLimitError = 0xFF09,
        PositionSensorBreachError = 0xFF0A,
        SystemOverloadedError = 0xFF0B,
        InterpolatedPositionModeError = 0xFF0C,
        AutoTuningIdentificationError = 0xFF0D,
        GearScalingFactorError = 0xFF0F,
        ControllerGainError = 0xFF10,
        MainSensorDirectionError = 0xFF11,
        AuxiliarySensorDirectionError = 0xFF12
    }

    /// <summary>
    /// Command codes.
    /// </summary>
    /// <remarks>
    /// A matching enumeration exists in firmware source.
    /// </remarks> 
    public enum CommandCodes
    {
        STOP = 0x00,                // 000
        mod_suiv_traj = 0x02,       // 002
        mode_libre = 0x04,          // 004
        mode_libre_visc = 0x20,
        mod_traj = 0x06,            // 006
        mod_homing = 0x08,          // 008
        mod_init_traj = 0x0A,       // 010
        mod_home = 0x0C,            // 012
        mod_game = 0x0E,            // 014
        heartbeat = 0x64,           // 100
        StreamingMod = 0xA8,        // 168
        //Borne = 0x8C,               // 140
        //BorneCirc = 0x8E,           // 142
        STOPnv = 0xF0,              // 240
        deviceID = 0xFA,            // 250
        deviceDate = 0xFC,          // 252
        deviceSerialNumber = 0xFE,  // 254
        Reset,
        Sleep
    }

    /// <summary>
    /// Config addresses.
    /// </summary>
    /// <remarks>
    /// A matching enumeration exists in firmware source.
    /// </remarks> 
    public enum ConfigAddresses
    {
        Jeu_Flash_Course = 0x54,    // 084
        Regulateur = 0x56,      // 086 (balle de golf, checkpoint de la course).
        Jeu_Flash_Golf = 0x58,      // 088
        Cibles = 0x5A,              // 090
        Mouvements = 0x5C,          // 092
        Formes = 0x5E,              // 094
        Jeux = 0x60,                // 096
        Libre = 0x62,               // 098
        mod_game_position = 0x14,   // 020
        mod_game_position_dyn = 0x16,   // 022
        MasseVisco = 0x64,          // 100
        KlatClong = 0x66,           // 102
        VitesseNbrsrep = 0x68,      // 104
        ActifPassif = 0x6A,         // 106
        Borne = 0x8C,               // 140
        BorneCirc = 0x8E,           // 142
        StreamingInit = 0xA0,       // 160
        StreamingSeg = 0xA2,        // 162
        StreamingPoint = 0xA4,      // 164
        StreamingError = 0xA6,      // 166
        StreamingNext = 0xAA,       // 170
        modMazeCalib = 0xAC,        // 172
        modMazeGame = 0xAE,        // 174
        mod_pos = 0x22,
        mod_hemi_pdr = 0x48
        // TODO : peut etre ajouter une trame pour passif...
    }

    public enum UniBiCodes
    {
        UnimanuelGauche = 0x00,
        UnimanuelDroite = 0x01,
        BimanuelGaucheXDroiteY = 0x02,
        BimanuelGaucheYDroiteX = 0x03,
    }
}
