// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Inputs/PlayerControlScheme.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControlScheme : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @PlayerControlScheme()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControlScheme"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""b07caab5-c562-4c9f-9828-9ddaa243aebe"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a264a7e8-86ce-4340-970f-b090c3d4ebc4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Bark"",
                    ""type"": ""Button"",
                    ""id"": ""147f17a4-b408-4210-b663-9b0076d3302b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Bite"",
                    ""type"": ""Button"",
                    ""id"": ""7e38ef6b-0048-4144-9c2e-21d86a6ffde1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Dig"",
                    ""type"": ""Button"",
                    ""id"": ""3af24f66-1e77-455f-977e-21075774ac20"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Talk"",
                    ""type"": ""Button"",
                    ""id"": ""73d93f39-af71-41c3-a24d-25d03a706ab1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""78d62108-047a-4f2b-bf7f-525365779f2e"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Bark"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bf3a049-bd25-4544-b689-fc2c2661bf6a"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Bark"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc75555f-e5e3-4726-b24e-a08772a41429"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Bite"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cbd2d7fb-33b4-4f4a-ae49-8242adda8eef"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Bite"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""f430d176-7f9e-437f-a3d2-383f995c7480"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""db81635a-090d-4976-8c2c-5faf3b48b6bc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""311b4a85-c3b4-4280-ab7b-e4f9f97d375c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a25003e1-2b12-4a17-a5ee-f74b19bd2ba2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""25c1473c-1c36-4309-b8d9-e35290296e16"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6ebddc4d-0ad9-484a-abe7-73e10b50e7df"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d51b127-7106-4faa-9e30-b9d39fa99339"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Dig"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b90b587-4f2b-4399-9d76-60c7e2853dbb"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Dig"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d38d2a2d-4054-40ae-b201-26df72964626"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Talk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb507cdb-432c-45a6-abf8-f049b5877876"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Talk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerBiting"",
            ""id"": ""246dec50-e351-453a-93ba-5bcb117fab8e"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""4d1d5c71-ed88-411d-9a88-f115f88e9b14"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Bite"",
                    ""type"": ""Button"",
                    ""id"": ""5e6d748c-eca1-40bd-a029-d01ba53a040b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e6bbf7ba-c0a7-4b02-8154-c6dd462aaa93"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Bite"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d6f8459-9be6-45f1-883c-6553a97f7510"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Bite"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""85667314-3c76-44d8-aee1-a0f16f206bb0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""40dc95ae-2a86-4864-8649-1645ca8900f4"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""547b147d-4be8-4692-9b2f-76ea3dcd54c2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fb9e7167-25ad-41db-908a-60cff3a25b65"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6abbfd3f-7929-4467-b7b8-e6d9ac2cf5f0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""586a4d7c-f0ff-410f-872c-9608b582a652"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2"",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player"",
            ""bindingGroup"": ""Player"",
            ""devices"": []
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Bark = m_Player.FindAction("Bark", throwIfNotFound: true);
        m_Player_Bite = m_Player.FindAction("Bite", throwIfNotFound: true);
        m_Player_Dig = m_Player.FindAction("Dig", throwIfNotFound: true);
        m_Player_Talk = m_Player.FindAction("Talk", throwIfNotFound: true);
        // PlayerBiting
        m_PlayerBiting = asset.FindActionMap("PlayerBiting", throwIfNotFound: true);
        m_PlayerBiting_Move = m_PlayerBiting.FindAction("Move", throwIfNotFound: true);
        m_PlayerBiting_Bite = m_PlayerBiting.FindAction("Bite", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Bark;
    private readonly InputAction m_Player_Bite;
    private readonly InputAction m_Player_Dig;
    private readonly InputAction m_Player_Talk;
    public struct PlayerActions
    {
        private @PlayerControlScheme m_Wrapper;
        public PlayerActions(@PlayerControlScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Bark => m_Wrapper.m_Player_Bark;
        public InputAction @Bite => m_Wrapper.m_Player_Bite;
        public InputAction @Dig => m_Wrapper.m_Player_Dig;
        public InputAction @Talk => m_Wrapper.m_Player_Talk;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Bark.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBark;
                @Bark.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBark;
                @Bark.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBark;
                @Bite.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBite;
                @Bite.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBite;
                @Bite.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBite;
                @Dig.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDig;
                @Dig.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDig;
                @Dig.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDig;
                @Talk.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTalk;
                @Talk.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTalk;
                @Talk.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTalk;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Bark.started += instance.OnBark;
                @Bark.performed += instance.OnBark;
                @Bark.canceled += instance.OnBark;
                @Bite.started += instance.OnBite;
                @Bite.performed += instance.OnBite;
                @Bite.canceled += instance.OnBite;
                @Dig.started += instance.OnDig;
                @Dig.performed += instance.OnDig;
                @Dig.canceled += instance.OnDig;
                @Talk.started += instance.OnTalk;
                @Talk.performed += instance.OnTalk;
                @Talk.canceled += instance.OnTalk;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // PlayerBiting
    private readonly InputActionMap m_PlayerBiting;
    private IPlayerBitingActions m_PlayerBitingActionsCallbackInterface;
    private readonly InputAction m_PlayerBiting_Move;
    private readonly InputAction m_PlayerBiting_Bite;
    public struct PlayerBitingActions
    {
        private @PlayerControlScheme m_Wrapper;
        public PlayerBitingActions(@PlayerControlScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerBiting_Move;
        public InputAction @Bite => m_Wrapper.m_PlayerBiting_Bite;
        public InputActionMap Get() { return m_Wrapper.m_PlayerBiting; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerBitingActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerBitingActions instance)
        {
            if (m_Wrapper.m_PlayerBitingActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerBitingActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerBitingActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerBitingActionsCallbackInterface.OnMove;
                @Bite.started -= m_Wrapper.m_PlayerBitingActionsCallbackInterface.OnBite;
                @Bite.performed -= m_Wrapper.m_PlayerBitingActionsCallbackInterface.OnBite;
                @Bite.canceled -= m_Wrapper.m_PlayerBitingActionsCallbackInterface.OnBite;
            }
            m_Wrapper.m_PlayerBitingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Bite.started += instance.OnBite;
                @Bite.performed += instance.OnBite;
                @Bite.canceled += instance.OnBite;
            }
        }
    }
    public PlayerBitingActions @PlayerBiting => new PlayerBitingActions(this);
    private int m_PlayerSchemeIndex = -1;
    public InputControlScheme PlayerScheme
    {
        get
        {
            if (m_PlayerSchemeIndex == -1) m_PlayerSchemeIndex = asset.FindControlSchemeIndex("Player");
            return asset.controlSchemes[m_PlayerSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnBark(InputAction.CallbackContext context);
        void OnBite(InputAction.CallbackContext context);
        void OnDig(InputAction.CallbackContext context);
        void OnTalk(InputAction.CallbackContext context);
    }
    public interface IPlayerBitingActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnBite(InputAction.CallbackContext context);
    }
}
